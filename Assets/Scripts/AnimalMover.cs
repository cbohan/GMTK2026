using System.Collections;
using System.Linq;
using UnityEngine;

public class AnimalMover : MonoBehaviour
{
    private static readonly int TilingHash = Shader.PropertyToID("_Tiling");
    private static readonly int OffsetHash = Shader.PropertyToID("_Offset");
    private static readonly int FlipXHash = Shader.PropertyToID("_FlipX");
    
    [SerializeField] private TiggerVolume _triggerVolume;
    [SerializeField] private AnimalMove[] _moves;
    [SerializeField] private Vector2Int _frames;
    [SerializeField] private int[] _runFrames;
    [SerializeField] private float _secondPerRunFrame = .25f;
    [SerializeField] private int[] _waitFrames;
    [SerializeField] private float _secondPerWaitFrame = .5f;

    private Material _material;
    private bool _hasBeenTriggered;
    private LayerMask layerMask;

    private void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;
        _material.SetVector(TilingHash, new Vector4(_frames.x, _frames.y, 0, 0));
        _material.SetVector(OffsetHash, new Vector4(0, 0, 0, 0));
        layerMask = LayerMask.GetMask("Terrain");
    }

    private void Start()
    {
        _triggerVolume?.OnTriggered.AddListener(() =>
        {
            StartCoroutine(Move());
        });
    }
    
    private IEnumerator Move()
    {
        foreach (var move in _moves)
        {
            var reachedTarget = false;
            var runFrameTimer = 0f;
            while (!reachedTarget)
            {
                var targetPosition = Vector3.MoveTowards(transform.position, move.Position, move.MoveSpeed * Time.deltaTime);
                if (Physics.Raycast(targetPosition + Vector3.up * 10f, Vector3.down, out var hit, Mathf.Infinity, layerMask))
                {
                    targetPosition = hit.point;
                }
                transform.position = targetPosition;
                reachedTarget = Vector3.Distance(transform.position, move.Position) < .1f;
                
                runFrameTimer += Time.deltaTime;
                var runFrame = Mathf.FloorToInt(runFrameTimer / _secondPerRunFrame) % _runFrames.Length;
                _material.SetVector(OffsetHash, GetOffset(_runFrames[runFrame]));
                _material.SetFloat(FlipXHash, IsMovingRight(transform.position, move.Position) ? 0f : 1f);
                
                yield return MoveTowardsNearbyTrash();
            }


            var waitTimer = 0f;
            while (waitTimer < move.WaitDuration)
            {
                waitTimer += Time.deltaTime;
                var waitFrame = Mathf.FloorToInt(waitTimer / _secondPerWaitFrame) % _waitFrames.Length;
                _material.SetVector(OffsetHash, GetOffset(_waitFrames[waitFrame]));
                
                yield return MoveTowardsNearbyTrash();
            }
        }

        yield return null;
    }

    private IEnumerator MoveTowardsNearbyTrash()
    {
        if (Trash.Trashes == null || Trash.Trashes.Count == 0) yield break;
        
        var nearestTrash = Trash.Trashes.OrderBy(trash => Vector3.Distance(trash.transform.position, transform.position)).First();
        var distanceToNearestTrash = Vector3.Distance(transform.position, nearestTrash.transform.position);
        var runToTrashDistance = 5f;
        
        if (distanceToNearestTrash > runToTrashDistance) yield break;
        
        var reachedTarget = false;
        var runFrameTimer = 0f;
        while (!reachedTarget)
        {
            var targetPosition = Vector3.MoveTowards(
                transform.position, 
                nearestTrash.Position, 
                3f * Time.deltaTime);
            if (Physics.Raycast(targetPosition + Vector3.up * 10f, Vector3.down, out var hit, Mathf.Infinity, layerMask))
            {
                targetPosition = hit.point;
            }
            transform.position = targetPosition;
            reachedTarget = Vector3.Distance(transform.position, nearestTrash.Position) < .5f;
                
            runFrameTimer += Time.deltaTime;
            var runFrame = Mathf.FloorToInt(runFrameTimer / _secondPerRunFrame) % _runFrames.Length;
            _material.SetVector(OffsetHash, GetOffset(_runFrames[runFrame]));
            _material.SetFloat(FlipXHash, IsMovingRight(transform.position, nearestTrash.Position) ? 0f : 1f);
                
            yield return null;
        }
        
        // Eat the trash
        yield return nearestTrash.Eat();
        
        yield return null;
    }

    private bool IsMovingRight(Vector3 start, Vector3 end)
    {
        var startScreenPoint = Camera.main.WorldToScreenPoint(start);
        var endScreenPoint = Camera.main.WorldToScreenPoint(end);
        return startScreenPoint.x < endScreenPoint.x;
    }

    private Vector4 GetOffset(int frame)
    {
        return new Vector4(frame % _frames.x, frame / _frames.x, 0, 0);
    }
    
    private void OnDrawGizmos()
    {
        var i = 0;
        var style = new GUIStyle
        {
            normal =
            {
                textColor = Color.red
            },
            fontSize = 14
        };
        
        Gizmos.color = Color.red;
        foreach (var move in _moves)
        {
            if (!move) continue;
#if UNITY_EDITOR
            // Position offset so text doesn't overlap the center point directly
            var textPosition = move.Position + Vector3.up;
            UnityEditor.Handles.Label(textPosition, $"{++i}", style);
#endif
        }
    }
}

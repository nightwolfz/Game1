using System.Collections;
using UnityEngine;

namespace Assets.Plugins
{
    public class TimedTrailRenderer : MonoBehaviour
    {

        public bool emit = true;
        public float emitTime = 0.00f;
        public Material material;

        public float lifeTime = 1.00f;

        public Color[] Colors;
        public float[] Sizes;

        public float UvLengthScale = 0.01f;
        public bool HigherQualityUVs = true;

        public int movePixelsForRebuild = 6;
        public float maxRebuildTime = 0.1f;

        public float minVertexDistance = 0.10f;

        public float maxVertexDistance = 10.00f;
        public float maxAngle = 3.00f;

        public bool autoDestruct = true;

        private ArrayList points = new ArrayList();
        private GameObject o;
        private Vector3 _lastPosition;
        private Vector3 _lastCameraPosition1;
        private Vector3 _lastCameraPosition2;
        private float _lastRebuildTime = 0.00f;
        private bool _lastFrameEmit = true;

        public class Point
        {
            public float TimeCreated = 0.00f;
            public Vector3 Position;
            public bool LineBreak = false;
        }

        void Start()
        {
            _lastPosition = transform.position;
            o = new GameObject("Trail");
            o.transform.parent = null;
            o.transform.position = Vector3.zero;
            o.transform.rotation = Quaternion.identity;
            o.transform.localScale = Vector3.one;
            o.AddComponent<MeshFilter>();
            o.AddComponent<MeshRenderer>();
            o.renderer.material = material;
			Destroy(o, lifeTime);
        }

        void OnEnable()
        {
            Start();
        }

        void OnDisable()
        {
            Destroy(o);
        }

        void Update()
        {
            if (emit && emitTime > 0)
            {
                emitTime -= Time.deltaTime;
                if (emitTime < 0.001) emit = false;
            }

            if (!emit && points.Count == 0 && autoDestruct)
            {
                Destroy(o);
            }

            // early out if there is no camera
            if (!Camera.main) return;

            bool re = false;

            // if we have moved enough, create a new vertex and make sure we rebuild the mesh
            float theDistance = (_lastPosition - transform.position).magnitude;
            if (emit)
            {
                if (theDistance > minVertexDistance)
                {
                    bool make = false;
                    if (points.Count < 3)
                    {
                        make = true;
                    }
                    else
                    {
                        Vector3 l1 = ((Point)points[points.Count - 2]).Position - ((Point)points[points.Count - 3]).Position;
                        Vector3 l2 = ((Point)points[points.Count - 1]).Position - ((Point)points[points.Count - 2]).Position;
                        if (Vector3.Angle(l1, l2) > maxAngle || theDistance > maxVertexDistance) make = true;
                    }

                    if (make)
                    {
                        Point p = new Point();
                        p.Position = transform.position;
                        p.TimeCreated = Time.time;
                        points.Add(p);
                        _lastPosition = transform.position;
                    }
                    else
                    {
                        ((Point)points[points.Count - 1]).Position = transform.position;
                        ((Point)points[points.Count - 1]).TimeCreated = Time.time;
                    }
                }
                else if (points.Count > 0)
                {
                    ((Point)points[points.Count - 1]).Position = transform.position;
                    ((Point)points[points.Count - 1]).TimeCreated = Time.time;
                }
            }

            if (!emit && _lastFrameEmit && points.Count > 0) ((Point)points[points.Count - 1]).LineBreak = true;
            _lastFrameEmit = emit;

            // approximate if we should rebuild the mesh or not
            if (points.Count > 1)
            {
                Vector3 cur1 = Camera.main.WorldToScreenPoint(((Point)points[0]).Position);
                _lastCameraPosition1.z = 0;
                Vector3 cur2 = Camera.main.WorldToScreenPoint(((Point)points[points.Count - 1]).Position);
                _lastCameraPosition2.z = 0;

                float distance = (_lastCameraPosition1 - cur1).magnitude;
                distance += (_lastCameraPosition2 - cur2).magnitude;

                if (distance > movePixelsForRebuild || Time.time - _lastRebuildTime > maxRebuildTime)
                {
                    re = true;
                    _lastCameraPosition1 = cur1;
                    _lastCameraPosition2 = cur2;
                }
            }
            else
            {
                re = true;
            }


            if (re)
            {
                _lastRebuildTime = Time.time;

                ArrayList remove = new ArrayList();
                int i = 0;
                foreach (Point p in points)
                {
                    // cull old points first
                    if (Time.time - p.TimeCreated > lifeTime) remove.Add(p);
                    i++;
                }

                foreach (Point p in remove) points.Remove(p);
                remove.Clear();

                if (points.Count > 1)
                {
                    Vector3[] newVertices = new Vector3[points.Count * 2];
                    Vector2[] newUV = new Vector2[points.Count * 2];
                    int[] newTriangles = new int[(points.Count - 1) * 6];
                    Color[] newColors = new Color[points.Count * 2];

                    i = 0;
                    float curDistance = 0.00f;

                    foreach (Point p in points)
                    {
                        float time = (Time.time - p.TimeCreated) / lifeTime;

                        Color color = Color.Lerp(Color.white, Color.clear, time);
                        if (Colors != null && Colors.Length > 0)
                        {
                            float colorTime = time * (Colors.Length - 1);
                            float min = Mathf.Floor(colorTime);
                            float max = Mathf.Clamp(Mathf.Ceil(colorTime), 1, Colors.Length - 1);
                            float lerp = Mathf.InverseLerp(min, max, colorTime);
                            if (min >= Colors.Length) min = Colors.Length - 1; if (min < 0) min = 0;
                            if (max >= Colors.Length) max = Colors.Length - 1; if (max < 0) max = 0;
                            color = Color.Lerp(Colors[(int)min], Colors[(int)max], lerp);
                        }

                        float size = 1f;
                        if (Sizes != null && Sizes.Length > 0)
                        {
                            float sizeTime = time * (Sizes.Length - 1);
                            float min = Mathf.Floor(sizeTime);
                            float max = Mathf.Clamp(Mathf.Ceil(sizeTime), 1, Sizes.Length - 1);
                            float lerp = Mathf.InverseLerp(min, max, sizeTime);
                            if (min >= Sizes.Length) min = Sizes.Length - 1; if (min < 0) min = 0;
                            if (max >= Sizes.Length) max = Sizes.Length - 1; if (max < 0) max = 0;
                            size = Mathf.Lerp(Sizes[(int)min], Sizes[(int)max], lerp);
                        }

                        Vector3 perpendicular = Vector3.up;//Vector3.Cross(lineDirection, vectorToCamera).normalized;

                        newVertices[i * 2] = p.Position + (perpendicular * (size * 0.5f));
                        newVertices[(i * 2) + 1] = p.Position + (-perpendicular * (size * 0.5f));

                        newColors[i * 2] = newColors[(i * 2) + 1] = color;

                        newUV[i * 2] = new Vector2(curDistance * UvLengthScale, 0);
                        newUV[(i * 2) + 1] = new Vector2(curDistance * UvLengthScale, 1);

                        if (i > 0 && !((Point)points[i - 1]).LineBreak)
                        {
                            if (HigherQualityUVs) curDistance += (p.Position - ((Point)points[i - 1]).Position).magnitude;
                            else curDistance += (p.Position - ((Point)points[i - 1]).Position).sqrMagnitude;

                            newTriangles[(i - 1) * 6] = (i * 2) - 2;
                            newTriangles[((i - 1) * 6) + 1] = (i * 2) - 1;
                            newTriangles[((i - 1) * 6) + 2] = i * 2;

                            newTriangles[((i - 1) * 6) + 3] = (i * 2) + 1;
                            newTriangles[((i - 1) * 6) + 4] = i * 2;
                            newTriangles[((i - 1) * 6) + 5] = (i * 2) - 1;
                        }

                        i++;
                    }

                    if (o != null)
                    {
                        Mesh mesh = (o.GetComponent<MeshFilter>() as MeshFilter).mesh;
                        mesh.Clear();
                        mesh.vertices = newVertices;
                        mesh.colors = newColors;
                        mesh.uv = newUV;
                        mesh.triangles = newTriangles;
                    }
                }
            }
        }
    }
}
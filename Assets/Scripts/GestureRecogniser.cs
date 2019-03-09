using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDollarGestureRecognizer;
using System.IO;
using AstroNet.GameElements;
using UnityEngine.EventSystems;
namespace AstroNet
{
    public class GestureRecogniser : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {

        [SerializeField] private Transform gestureOnScreenPrefab;

        [SerializeField] private AssembledObject _assembledObject;

        private List<Gesture> trainingSet = new List<Gesture>();

        private List<Point> points = new List<Point>();
        private int strokeId = -1;

        private Vector3 virtualKeyPosition = Vector2.zero;
        private Rect drawArea;

        private RuntimePlatform platform;
        private int vertexCount = 0;

        private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>();
        private LineRenderer currentGestureLineRenderer;

        private bool recognized;

        private bool pointerDown;

        public void OnPointerDown(PointerEventData eventData)
        {
            pointerDown = true;
            if (recognized)
            {

                recognized = false;
                strokeId = -1;

                points.Clear();

                foreach (LineRenderer lineRenderer in gestureLinesRenderer)
                {

                    lineRenderer.SetVertexCount(0);
                    Destroy(lineRenderer.gameObject);
                }

                gestureLinesRenderer.Clear();
            }

            ++strokeId;

            Transform tmpGesture = Instantiate(gestureOnScreenPrefab, transform.position, transform.rotation) as Transform;
            currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();

            gestureLinesRenderer.Add(currentGestureLineRenderer);

            vertexCount = 0;

            Debug.Log("OnPointerDoown");
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            pointerDown = false;
            RecogniseDrawing();
            Debug.Log("OnPointerUp");
        }

        // Start is called before the first frame update
        void Start()
        {
            platform = Application.platform;
            drawArea = new Rect(0, 0, Screen.width - Screen.width / 3, Screen.height);

            var files = Resources.LoadAll<TextAsset>("Gestures/") as TextAsset[];
            foreach (TextAsset file in files)
                trainingSet.Add(GestureIO.ReadGestureFromXML(file.text));
        }

        private void RecogniseDrawing()
        {
            recognized = true;

            Gesture candidate = new Gesture(points.ToArray());
            Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());

            Debug.Log(gestureResult.GestureClass + " " + gestureResult.Score);

            if (gestureResult.GestureClass.Equals("square"))
            {
                _assembledObject.Attack(FaceType.Square);
            }
            else if (gestureResult.GestureClass.Equals("circle"))
            {
                var clone = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                clone.transform.position = Vector3.zero;
            }
        }
        // Update is called once per frame
        void Update()
        {


            if (pointerDown)
            {
                if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)
                {
                    virtualKeyPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
                }
                else
                {
                    virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
                }

                if (drawArea.Contains(virtualKeyPosition))
                {

                    if (Input.GetMouseButton(0))
                    {
                        points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));

                        currentGestureLineRenderer.positionCount = ++vertexCount;
                        currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));
                    }
                }
            }
        }
    }
}

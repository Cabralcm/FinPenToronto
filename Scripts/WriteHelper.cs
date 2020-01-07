using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.WSA.Input;
using UnityEngine.XR.WSA;
using UnityEngine.VR;
using UnityEngine.Windows.Speech;
using System.Linq;
using System;


public class WriteHelper : MonoBehaviour
{
    public GameObject gameObject;
    public GameObject camera;
    public static List<GameObject> SingleDraw = new List<GameObject>();
    public List<List<GameObject>> AllDrawings = new List<List<GameObject>>();
    private Material m_Material;
    private Material tempMaterial;

    private Color current_color;
    private int rainbowActive;
    private int rainbowInt;
    private float ref_distance;

    bool _handTracked = false;
    private GameObject _crs;
    public bool showHandCrsr = true;

    public GameObject CurFocusedObj { get; set; }
    
    static GestureRecognizer tapRecognizer;
    static InteractionSourcePose poseTest;

    private TappedEventArgs testObj;

    private List<GameObject> Individual = new List<GameObject>();

    private Color[] ColorBank = { new Color(1, 1, 1, 1), //Color = White
                                new Color(1, 0, 0, 1), //Color = Red
                                new Color(1, 0.92f, 0.016f, 1), //Color = Yellow
                                new Color(0, 1, 0, 1), //Color = Green
                                new Color(0, 0, 1, 1), //Color = Blue
                                new Color(0.5f, 0.5f, 0.5f, 1), //Color = Grey
                                new Color(0, 0, 0, 1), //Color = Black
                                new Color(0.4117f, 0.4117f, 0.4117f, 1)}; 
    private Color[] ColorWheel = {
                                new Color (1,0,0,1),    //Red
                                new Color(1,0,1,1),     //Magenta
                                new Color (0,0,1,1),    //Blue
                                new Color (0,1,1,1),    //Turqoise
                                new Color (0,1,0,1),    //Green
                                new Color(1, 0.92f, 0.016f, 1), //Yellow
                                new Color (1,0.647f,0,1)};  //Orange
    private List<Color> ReferenceColors = new List<Color>();
    //private Color[] ReferenceColors = new Color[0];

    private int writeState = 0;
    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    private Vector3 handPos;
    private Vector3 handForward;
    private Quaternion handRotation;
    private InteractionSource hand;

    public static int numOfPoints;
    public static int previousNumPoints;
    private int addedPoints;
    public static List<int> drawingLengths = new List<int>();

    private Vector3[] linePositions = new Vector3[0];
    private Vector3[] linePositionsOld = new Vector3[0];
    public float lineSegmentSize = 0.15f;
    public float lineWidth = 0.1f;


    void Awake()
    {
        InteractionManager.InteractionSourceDetected += InteractionManager_InteractionSourceDetected;
        InteractionManager.InteractionSourceLost += InteractionManager_InteractionSourceLost;
        InteractionManager.InteractionSourceUpdated += InteractionManager_InteractionSourceUpdated;
        InteractionManager.InteractionSourcePressed += InteractionManager_InteractionSourcePressed;
    }

    private void InteractionManager_InteractionSourcePressed(InteractionSourcePressedEventArgs obj)
    {
        throw new NotImplementedException();
    }

    //Obtains the position/location of the User's Hand
    private void InteractionManager_InteractionSourceUpdated(InteractionSourceUpdatedEventArgs obj)
    {
        uint id = obj.state.source.id;
        if(obj.state.source.kind == InteractionSourceKind.Hand){
            obj.state.sourcePose.TryGetPosition(out handPos);
            obj.state.sourcePose.TryGetForward(out handForward);
            obj.state.sourcePose.TryGetRotation(out handRotation);
            Debug.Log("Object Position: " + handPos);
        }
    }
    //Function is required
    private void InteractionManager_InteractionSourceLost(InteractionSourceLostEventArgs obj)
    {
        //throw new NotImplementedException();
    }
    //Function is required
    private void InteractionManager_InteractionSourceDetected(InteractionSourceDetectedEventArgs obj)
    {
        //throw new NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        current_color = ColorBank[0];
        rainbowActive = 0;
        rainbowInt = 0;
        ref_distance = 20.0f;

        if (showHandCrsr)
        {
            _crs = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            _crs.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
            _crs.GetComponent<Renderer>().material.color = Color.green;
            _crs.GetComponent<Collider>().enabled = false;
            _crs.SetActive(false);
        }

        numOfPoints = 0;
        tapRecognizer = new GestureRecognizer();
        poseTest = new InteractionSourcePose();
        tapRecognizer.SetRecognizableGestures(GestureSettings.Tap);
        tapRecognizer.Tapped += TapRecognizer_Tapped;
        tapRecognizer.StartCapturingGestures();

        KeywordRecognizer keywordRecognizer_erase = new KeywordRecognizer(new[] { "erase" });
        keywordRecognizer_erase.OnPhraseRecognized += KeywordRecognizer_erase_OnPhraseRecognized;
        keywordRecognizer_erase.Start();

        KeywordRecognizer keywordRecognizer_restart = new KeywordRecognizer(new[] { "restart" });
        keywordRecognizer_restart.OnPhraseRecognized += KeywordRecognizer_restart_OnPhraseRecognized;
        keywordRecognizer_restart.Start();
        
        keywords.Add("Erase", () =>
        {
            Debug.Log("erase word recognized!");
            if (writeState == 0)
            {
                DeleteDrawing();
            }

            Debug.Log("restart recognized! THIS IS THE SECOND METHOD!");
        });
        keywords.Add("Clear all", () =>
        {
            Debug.Log("Clear all word recognized");
            if (writeState == 0)
            {
                DeleteAll();
            }
        });
        
        //Add Voice Recognition for Changing Color to: Blue
        keywords.Add("Color Blue", () =>
        {
            Debug.Log("Color changed to Blue");
            current_color = ColorBank[4];
            ////Keep Code below for example with Shading and Color.Lerp feature
            //if (writeState == 0)
            //{
            //    int ref_point;
            //    int last_index = drawingLengths.Count - 1;
            //    ref_point = numOfPoints - drawingLengths[last_index];

            //    for (int i = SingleDraw.Count - 1; i >= ref_point; i--)
            //    {
            //        Renderer rend = SingleDraw[i].GetComponent<Renderer>();
            //        m_Material = rend.material; ;
            //        m_Material.color = ColorBank[4];
            //    }
            //}
        });
        //Add Voice Recognition for Changing Color to: Red
        keywords.Add("Color Red", () =>
        {
            Debug.Log("Color changed to Red");
            rainbowActive = 0;
            current_color = ColorBank[1];
        });
        //Add Voice Recognition for Changing Color to: Green
        keywords.Add("Color Green", () =>
        {
            Debug.Log("Color changed to Green");
            rainbowActive = 0;
            current_color = ColorBank[3];
        });
        //Add Voice Recognition for Changing Color to: Yellow
        keywords.Add("Color Yellow", () =>
        {
            Debug.Log("Color changed to Yellow");
            rainbowActive = 0;
            current_color = ColorBank[2];
        });
        //Add Voice Recognition for Changing Color to: White
        keywords.Add("Color White", () =>
        {
            Debug.Log("Color changed to White");
            rainbowActive = 0;
            current_color = ColorBank[0];
        });
        keywords.Add("Color Grey", () =>
        {
            Debug.Log("Color changed to Grey");
            rainbowActive = 0;
            current_color = ColorBank[5];

        });
        keywords.Add("Color Gray", () =>
        {
            Debug.Log("Color changed to Gray");
            rainbowActive = 0;
            current_color = ColorBank[5];
        });

        //Add Rainbow Wheel of Colors
        keywords.Add("Color Rainbow", () =>
        {
            rainbowActive = 1;

        });
        //Add Voice Recognition for Start Drawing:
        keywords.Add("Start Drawing", ()=>
        {
            Debug.Log("Start Drawing Initiated");
            if (writeState == 0) //Turn ON writing
            {
                writeState = 1;
            }
        });
        //Add Voice Recognition for Stop Drawing:
        keywords.Add("Stop Drawing", ()=>
        {
            Debug.Log("Stop Drawing Initiated");
            if (writeState == 1)//Turn OFF writing
            {
                //Save the number of points for a single drawing in an int array
                addedPoints = SingleDraw.Count - numOfPoints;
                previousNumPoints = previousNumPoints + addedPoints;
                drawingLengths.Add(SingleDraw.Count - numOfPoints);
                
                //Update numOfPoints
                numOfPoints = SingleDraw.Count;

                Debug.Log("Size of SingleDraw BEFORE clear: " + SingleDraw.Count());
                Debug.Log("Size of AllDrawings: " + AllDrawings.Count());
                Debug.Log("Size of SingleDraw AFTER clear: " + SingleDraw.Count());
                writeState = 0;
            }
        });

        //Save all of the Keywords into the KeywordRecognizer
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();

    }

    public static List<GameObject> getDrawingPoints()
    {
        return SingleDraw;
    }

    public static List<int> getDrawingLengths()
    {
        return drawingLengths;
    }

    public void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Action keywordAction;
        if(keywords.TryGetValue(args.text, out keywordAction))
        {
            //Program control goes back up to the "keywords.Add() =>" that matches the phrase spoken
            keywordAction.Invoke(); 
        }
    }

    void DeleteDrawing(){
        int last_index = drawingLengths.Count - 1;
        numOfPoints = numOfPoints - drawingLengths[last_index];
        //Color[] temp_colors = new Color[0];
        List<Color> temp_colors = new List<Color>();

        if (drawingLengths.Any()) //prevent IndexOutOfRangeException for empty list
        {
            drawingLengths.RemoveAt(last_index);
        }

        for (int i = SingleDraw.Count - 1; i >= numOfPoints; i--)
        {
            Destroy(SingleDraw[i]);
        }
        for(int i = 0; i <= numOfPoints; i++)
        {
            //temp_colors.Append(ReferenceColors[i]);
            temp_colors.Add(ReferenceColors[i]);
        }
        ReferenceColors = temp_colors;
    }

    void DeleteAll()
    {
        for (int i = SingleDraw.Count - 1; i >= 0; i--)
        {
            Destroy(SingleDraw[i]);
        }
        numOfPoints = 0;
        drawingLengths.Clear();
        //ReferenceColors = new Color[0];
        ReferenceColors.Clear();
    }

    private void KeywordRecognizer_restart_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Restart was received");
        for (int i = SingleDraw.Count - 1; i >= 0; i--)
        {
            Destroy(SingleDraw[i]);
        }
    }

    private void KeywordRecognizer_erase_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {}

    private void TapRecognizer_Tapped(TappedEventArgs obj)
    {
        testObj = obj;
        if (writeState == 0) //Turn ON writing
        {
            writeState = 1;
        }

        else if (writeState == 1)//Turn OFF writing
        {
            //Save the number of points for a single drawing in an int array
            drawingLengths.Add(SingleDraw.Count - numOfPoints);
            //Update numOfPoints
            numOfPoints = SingleDraw.Count;

            //Debug.Log("Size of SingleDraw BEFORE clear: " + SingleDraw.Count());
            //Debug.Log("Size of AllDrawings: " + AllDrawings.Count());
            //Debug.Log("Size of SingleDraw AFTER clear: " + SingleDraw.Count());
            writeState = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (writeState == 1) //Check if writing state is active
        {
            //Debug.Log("Touched");
            Vector3 camPos = camera.transform.position;
            Vector3 camDirection = camera.transform.forward;
            Quaternion camRotation = camera.transform.rotation;
            Vector3 handDirection = handPos - camPos;
            float spawnDistance = 2;
            //Debug.Log("Touched" + camPos.x + " " + camPos.y + " " + camPos.z);
            //Vector3 spawnPos = camPos + (camDirection * spawnDistance);
            Vector3 spawnPos = handPos * 3 + (camDirection * spawnDistance);
            //GameObject cur = Instantiate(gameObject, spawnPos, camRotation);
            GameObject cur = Instantiate(gameObject, spawnPos, camRotation);
            SingleDraw.Add(cur);
            cur.transform.SetParent(this.transform);
            if (rainbowActive == 0)
            {
                Renderer rend = cur.GetComponent<Renderer>();
                m_Material = rend.material;
                m_Material.color = current_color;
                ReferenceColors.Add(current_color);
            }
            else if (rainbowActive == 1)
            {
                Renderer rend = cur.GetComponent<Renderer>();
                m_Material = rend.material;
                if(rainbowInt >= ColorWheel.Length - 1)
                {
                    rainbowInt = 0;
                    ReferenceColors.Add(ColorWheel[rainbowInt]);
                    m_Material.color = ColorWheel[rainbowInt];
                }
                else
                {
                    rainbowInt++;
                    ReferenceColors.Add(ColorWheel[rainbowInt]);
                    m_Material.color = ColorWheel[rainbowInt];
                }
            }
            Renderer tempRend = cur.GetComponent<Renderer>();
            tempMaterial = tempRend.material;
            Color tempColor = tempMaterial.GetColor("_Color");
            //Color refColor = tempColor;
            //ReferenceColors.Add(tempColor);
        }
        //if(SingleDraw.Count >= 1)
        //{
        for (int i = (SingleDraw.Count - 1); i >= 0; i--)
        {
            GameObject curObj = SingleDraw[i];
            Vector3 curPos = curObj.transform.position;
            Vector3 camPos = camera.transform.position;
            float dist = Vector3.Distance(curPos, camPos);
            Debug.Log("Distance: " + dist);
            float t;
            if ((dist / ref_distance) > 1){
                t = 1.0f;
            }
            else{
                t = (dist / ref_distance);
            }
            //Debug.Log("Current T variable: " + t);
            //Debug.Log("Current Index: " + i);
            Debug.Log("Black: " + ColorBank[6]);
            //Debug.Log("Current Color: " + ReferenceColors[i]);
            Renderer rend = SingleDraw[i].GetComponent<Renderer>();
            m_Material = rend.material;
            //Color check_color = m_Material.GetColor("_Color");
            //Debug.Log("Current Color: " + check_color);
            //Debug.Log("Length of Ref Colors: " + ReferenceColors.Count);
            //Debug.Log("Reference Color: " + ReferenceColors[i]);
            //Debug.Log("BEFORE");
            m_Material.color = Color.Lerp(ReferenceColors[i], ColorBank[6], t);
            //Debug.Log("AFTER");
            //Color update_color = m_Material.GetColor("_Color");
            //Debug.Log("Updated Color: " + update_color);
        }
        //}
    }
}

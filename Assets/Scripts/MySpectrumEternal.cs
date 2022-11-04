using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//-----------------------------------------------------------------------------
// name: Spectrum.cs
// desc: set up and draw the spectrum history in stacked circles
//-----------------------------------------------------------------------------
public class MySpectrumEternal : MonoBehaviour
{
    // prefab reference
    public GameObject sphere;
    // array of spheres
    public GameObject[] the_spheres = new GameObject[512];

    // create 2d array of spectrum history
    int arrHeight = 8;
    int arrWidth = 512;
    public GameObject[,] spectrum_history = new GameObject[12, 512]; // 8 rows // 512 columns

    // central point that circles are spread around
    Vector3 point = new Vector3(0, 0, 0);
    Vector3 origin = new Vector3(0, 0, 0);

    // spectrum history matrix
    public float[] history = new float[512];

    // radius for circle
    int radius = 150;
    // twist start of circle by this amount
    public float twister = 0f;

    // set time values for spawn freq
    private float nextActionTime = 0.0f;
    public float period = .5f;

    Color blue = new Color(0f, 0f, 1f);
    Color [] colors = new Color[512];
    public Gradient gradient;

    // find min and max value of spectrum
    float minSpectrum = 0f;
    float maxSpectrum = 0f;
    float offset = 50f;

    // this function modifies the spectrum to favor lower frequencies
    void modSpectrum()
    {
        // local reference to the spectrum
        float[] spectrum = ChunityAudioInput.the_spectrum;

        int j = 0;
        // always assign the same value twice to the history and stop at 256
        for (int i = 0; i < spectrum.Length / 2; i++)
        {
            history[j] = spectrum[i] * 100;
            history[j + 1] = spectrum[i] * 100;
            j += 2;
        }
    }
    
    void initHistory()
    {
        // place the spheres initially - 8 circles
        for (int row = 0; row < spectrum_history.GetLength(0); row++)
        {
            for (int col = 0; col < spectrum_history.GetLength(1); col++)
            {
                // distance around the circle  
                var radians = 2 * Mathf.PI / arrWidth * col + twister;
                // Vector direction
                var vertical = Mathf.Sin(radians);
                var horizontal = Mathf.Cos(radians); 
                // make direction vector
                var spreadDir = new Vector3 (vertical, horizontal, 0);
                // get the spread position and add radius as the distance from the center point
                var spreadPos = point + spreadDir * radius;
                // instantiate new sphere
                GameObject go = Instantiate (sphere, spreadPos, Quaternion.identity);
                // set color of sphere
                go.GetComponent<Renderer>().material.SetColor("_Color", blue);
                // give it a name
                go.name = "sphere" + col;
                // set this as a child of this Spectrum
                go.transform.parent = this.transform;
                // put into array
                spectrum_history[row, col] = go;

                // get value of spectrum at pos row in the array
                float factor = 600 * Mathf.Sqrt(history[col]);

                float xx = spectrum_history[row, col].transform.localPosition.x;  
                float yy = spectrum_history[row, col].transform.localPosition.y; 
                float zz = spectrum_history[row, col].transform.localPosition.z + factor; 
                        
                spectrum_history[row, col].transform.localPosition = new Vector3(xx, yy, zz);
            }
            float x = point.x;  
            float y = point.y; 
            float z = point.z + offset; 
            point = new Vector3(x, y, z);
        }
        Debug.Log("inside set circle");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // set first spectrum circle
        initHistory();
        modSpectrum();
    }

    // Update is called once per frame
    void Update()
    {  
        // switch from lined-up to sediment spectrum history
        if (Input.GetKeyDown(KeyCode.L))
        {
            for (int row = 0; row < spectrum_history.GetLength(0); row++)
            {
                for (int col = 0; col < spectrum_history.GetLength(1); col++)
                {
                    Destroy(spectrum_history[row, col]);
                }
            }
            GetComponent<MySpectrum>().enabled = true;
            GetComponent<MySpectrumEternal>().enabled = false;  
        }

        modSpectrum();

        // move every array down a row in the 2d array - float into the distance
        for (int row = spectrum_history.GetLength(0) - 1; row >= 0; row--)
        {
            for (int col = 0; col < spectrum_history.GetLength(1); col++)
            {   
                if (row == 0)
                {
                    // get value of spectrum at pos row in the array
                    float factor = 600 * Mathf.Sqrt(history[col]);

                    float x = spectrum_history[row, col].transform.localPosition.x;  
                    float y = spectrum_history[row, col].transform.localPosition.y;
                    
                    spectrum_history[row, col].transform.localPosition = new Vector3(x, y, 0 + factor);
                    
                    // set color of game object
                    Debug.Log("his " + history[col]);
                    float normalizedFloat = Mathf.Clamp(history[col] , 0, 1) * 10000;
                    Debug.Log("norm " + normalizedFloat);
                    colors[col] = gradient.Evaluate(normalizedFloat);
                    spectrum_history[row, col].GetComponent<Renderer>().material.SetColor("_BaseColor", colors[col]);
                    spectrum_history[row, col].GetComponent<Renderer>().material.SetColor("_EmissionColor", colors[col]);
                }
                else 
                {
                    float x = spectrum_history[row - 1, col].transform.localPosition.x; 
                    float y = spectrum_history[row - 1, col].transform.localPosition.y; 
                    float z = spectrum_history[row - 1, col].transform.localPosition.z + offset; 

                    spectrum_history[row, col].transform.localPosition = new Vector3(x, y, z);

                    // set color of game object
                    float normalizedFloat = Mathf.Clamp(history[col] , 0, 1) * 10000;
                    colors[col] = gradient.Evaluate(normalizedFloat);
                    spectrum_history[row, col].GetComponent<Renderer>().material.SetColor("_BaseColor", colors[col]);
                    spectrum_history[row, col].GetComponent<Renderer>().material.SetColor("_EmissionColor", colors[col]);
                }
                // rotate spectrum history around the origin at 20 degrees/second.
                spectrum_history[row, col].transform.RotateAround(point, Vector3.forward, 20 * Time.deltaTime);
            }
        Debug.Log("after outer loop");
        }
    }
}

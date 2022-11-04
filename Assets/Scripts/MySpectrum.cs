using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//-----------------------------------------------------------------------------
// name: Spectrum.cs
// desc: set up and draw thw spectrum history in sedimenting dots
//-----------------------------------------------------------------------------
public class MySpectrum : MonoBehaviour
{
    // prefab reference
    public GameObject sphere;
    // array of spheres
    public GameObject[] the_spheres = new GameObject[512];
    // central point that circles are spread around
    Vector3 point = new Vector3(0, 0, 0);

    // radius for circle
    int radius = 150;
    // twist start of circle by this amount
    public float twister = 0f;

    // set time values for spawn freq
    private float nextActionTime = 0.0f;
    public float period = 1f;

    // set two initial colors
    Color red = new Color(1f, .5f, .5f);
    Color blue = new Color(.5f, .5f, 1f);
    Color green = new Color(.5f, .1f, .5f);
    Color curColor = new Color(.5f, 1f, .5f);

    Color [] colors = new Color[512];
    public Gradient gradient;

    // find min and max value of spectrum
    float minSpectrum = 0f;
    float maxSpectrum = 0f;

    void setCircle(float twister)
    {
        Color random = gradient.Evaluate(Random.Range(0f, 1f));
        // place the spheres initially
        for ( int i = 0; i < the_spheres.Length; i++ )
        {
            // distance around the circle  
            var radians = 2 * Mathf.PI / the_spheres.Length * i + twister;
            
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
            go.GetComponent<Renderer>().material.SetColor("_Color", random);
            // give it a name
            go.name = "sphere" + i;
            // set this as a child of this Spectrum
            go.transform.parent = this.transform;
            // put into array
            the_spheres[i] = go;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // set first spectrum circle
        setCircle(0);
    }

    // Update is called once per frame
    void Update()
    {
        // switch from lined-up to sediment spectrum history
        if (Input.GetKeyDown(KeyCode.K))
        {
            GetComponent<MySpectrumEternal>().enabled = true;
            GetComponent<MySpectrum>().enabled = false;
        }
        
        // set a new circle after x time
        if (Time.time > nextActionTime ) {
            nextActionTime += period;
            twister += 1f;
            setCircle(twister); // twist start point every frame
        }

        // local reference to the spectrum
        float[] spectrum = ChunityAudioInput.the_spectrum;

        // let spectrum history float away into the background
        for(int i = 0; i < the_spheres.Length; i++)
        {
            // find min and max of spectrum
            if (spectrum[i] > maxSpectrum){
                maxSpectrum = spectrum[i];
            }

            // set position of spheres
            float factor = 2000 * Mathf.Sqrt(spectrum[i]);

            float x = the_spheres[i].transform.localPosition.x;  
            float y = the_spheres[i].transform.localPosition.y; 
            float z = the_spheres[i].transform.localPosition.z; 

            the_spheres[i].transform.localPosition = new Vector3(x, y, z + factor);

            // set color
            // calculate the normalized float
            float normalizedFloat = Mathf.Clamp(spectrum[i] / maxSpectrum , 0, 1) * 25000;

            colors[i] = gradient.Evaluate(normalizedFloat);
            the_spheres[i].GetComponent<Renderer>().material.SetColor("_EmissionColor", colors[i]);
            the_spheres[i].GetComponent<Renderer>().material.SetColor("_BaseColor", colors[i]);

            // rotate spectrum history around the origin at 10 degrees / second.
            the_spheres[i].transform.RotateAround(point, Vector3.forward, 10 * Time.deltaTime); 
        }
    }
}

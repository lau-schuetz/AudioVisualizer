using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//-----------------------------------------------------------------------------
// name: Waveform.cs
// desc: set up and draw the audio waveform
//-----------------------------------------------------------------------------
public class MyWaveform : MonoBehaviour
{
    private int num_spheres = 480;
    // prefab reference
    public GameObject sphere_wav;
    // array of game objects - half circle
    public GameObject[] the_spheres_wave = new GameObject[480];
    // array of game objects - half circle - mirrored waveform 
    public GameObject[] the_spheres_mirror = new GameObject[480];


    // central point that circles are spread around
    Vector3 point = new Vector3(0, 0, 0);
    // controllable scale
    public float MY_SCALE = 10;
    // radius for circle
    public float radius = 150;

    Color white = new Color(1f, 1f, 1f);

    private void setWaveform() 
    {
        // place the first 512 spheres
        for (int i = 0; i < num_spheres; i++)
        {
            // distance around the circle  
            var radians = 2 * Mathf.PI / (num_spheres * 2) * i;
            // Vector direction
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians); 
            // make direction vector
            var spreadDir = new Vector3 (horizontal, 0, vertical);
            // get the spread position and add radius as the distance from the center point
            var spreadPos = point + spreadDir * radius;
            // instantiate new sphere
            GameObject sp = Instantiate (sphere_wav, spreadPos, Quaternion.identity);
            // set color of sphere
            sp.GetComponent<Renderer>().material.SetColor("_BaseColor", white);
            // give it a name
            sp.name = "sphere_wave" + i;
            // set this as a child of the Waveform
            sp.transform.parent = this.transform;
            // put into array
            the_spheres_wave[i] = sp;
        }
    }

    
    private void setWaveformMirror() 
    {
        // place the second 512 spheres, but play them in reverse order
        for (int i = 0; i < num_spheres; i++)
        {
            float offset = 2 * Mathf.PI / (num_spheres * 2) * num_spheres;
            // distance around the circle  
            var radians = (2 * Mathf.PI / (num_spheres * 2) * i) + offset;
            // Vector direction
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians); 
            // make direction vector
            var spreadDir = new Vector3 (horizontal, 0, vertical);
            // get the spread position and add radius as the distance from the center point
            var spreadPos = point + spreadDir * radius;
            // instantiate new sphere
            GameObject go = Instantiate (sphere_wav, spreadPos, Quaternion.identity);
            // set color of sphere
            go.GetComponent<Renderer>().material.SetColor("_BaseColor", white);
            // give it a name
            go.name = "sphere_mir" + i;
            // set this as a child of this Spectrum
            go.transform.parent = this.transform;
            // put into array
            the_spheres_mirror[i] = go;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        setWaveform();
        setWaveformMirror();
    }

    // Update is called once per frame
    void Update()
    {
        // local reference to the time domain waveform
        float[] wf = ChunityAudioInput.the_waveform;

        // position the spheres in first half
        for (int i = 0; i < num_spheres; i++)
        {
            the_spheres_wave[i].transform.localPosition =
                new Vector3(the_spheres_wave[i].transform.localPosition.x,
                            MY_SCALE * wf[i],
                            the_spheres_wave[i].transform.localPosition.z);
        }

        // position the spheres in second half, but mirror the waveform values
        for (int i = 0; i < num_spheres; i++)
        {
            int j = 479 - i;
            the_spheres_mirror[i].transform.localPosition =
                new Vector3(the_spheres_mirror[i].transform.localPosition.x,
                            MY_SCALE * wf[j],
                            the_spheres_mirror[i].transform.localPosition.z);
        }
    }
}

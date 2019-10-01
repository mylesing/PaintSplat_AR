using UnityEngine;
using System.Collections.Generic;
using Photon;
using UnityEngine.UI;

public class BoardBehavior : Photon.MonoBehaviour {

    public GameObject SplatterPrefab;
    public GameObject imageTarget;
    public int numhits;
    public Text winText;
    public Text lossText;

    private List<GameObject> splatters = new List<GameObject>();

    void Start()
    {
        winText.enabled = false;
        lossText.enabled = false;
        numhits = 0;
    }

    private void Update () {
	    if (Input.GetKeyDown(KeyCode.Space)) {
            imageTarget.SetActive(!imageTarget.activeSelf);
        }

        if (numhits > 10) {
                 lossText.enabled = true;
        } else if ((numhits <= 10) && (Score.currScore >= 10000.0f)) {
                 winText.enabled = true;
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
       
        var other = collision.collider.gameObject;
        Vector3 hit_position = other.transform.position;
        if (other.CompareTag("Ball"))
        {
            // add another hit
            numhits += 1;
            Debug.Log(numhits);

            PhotonNetwork.Destroy(other);
            Quaternion rot =  Quaternion.AngleAxis(Random.Range(0f, 360f), new Vector3(0, 0, 1)) ; //*transform.rotation;
            var splatter = Instantiate(SplatterPrefab, hit_position, rot) as GameObject;

            splatter.GetComponent<Renderer>().material.color = other.GetComponent<Renderer>().material.color;

            Score.currScore += Mathf.Floor(1000.0f - Mathf.Abs(Vector3.Distance(hit_position, new Vector3(0, 0, 0))));

            splatters.Add(splatter);
        }

    }
}

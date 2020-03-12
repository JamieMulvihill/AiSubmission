using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarConstructor : MonoBehaviour
{
    public GameObject car;
    public GameObject carBody;
    public GameObject backWheel;
    public GameObject frontWheel;

    public Rigidbody2D backWheelRigidbody;
    public Rigidbody2D frontWheelRigidbody;

    public WheelJoint2D backWheelJoint;
    public WheelJoint2D frontWheelJoint;

    public SpriteRenderer sprite;

    float carBodyScale;
    float backWheelScale;
    float frontWheelScale;
    float anchorPosXBack;
    float anchorPosXFront;
    float speed;

    float a, b, c, d, x, y;
    public float[] genes;

    // Start is called before the first frame update
    void Awake()
    {
        // output = (input - inputStartRange) / (inputEndRang - inputStartRange) * (outputEndRange - outputEndRange) + ouputEndRange; 
        //y = (x - a) / (b - a) * (d - c) + c;

        carBodyScale = Random.Range(1.25f, 3f);
        genes[0] = carBodyScale;
        backWheelScale = Random.Range(.25f, .75f);
        genes[1] = backWheelScale;
        frontWheelScale = Random.Range(.25f, .75f);
        genes[2] = frontWheelScale;
        anchorPosXBack = Random.Range(.1f, .9f);
        genes[3] = anchorPosXBack;
        anchorPosXFront = Random.Range(.1f, .9f);
        genes[4] = anchorPosXFront;
        speed = Random.Range(.5f, 1.75f);
        genes[5] = speed;

        Construct();

        //Color carColor = new Color( Random.Range(0f, 1f), Random.Range(0f, 1f),Random.Range(0f, 1f));
        Color carColor = new Color(0, 0, 1);

        sprite.color = carColor;
    }

    public void Construct() {
        carBody.transform.localScale = new Vector3(genes[0], 1, 1);

        backWheel.transform.localScale = new Vector3(genes[1], genes[1], 1);
        frontWheel.transform.localScale = new Vector3(genes[2], genes[2], 1);

        backWheelJoint.anchor = new Vector2(-genes[3], -carBody.transform.localScale.y);
        frontWheelJoint.anchor = new Vector2(genes[4], -carBody.transform.localScale.y);

        backWheelJoint.connectedAnchor = new Vector2(/*-carBody.transform.localScale.x / 2 * anchorPosXBack, -carBody.transform.localScale.y / 2*/ 0, 0);
        frontWheelJoint.connectedAnchor = new Vector2(/*carBody.transform.localScale.x / 2 * anchorPosXFront, -carBody.transform.localScale.y / 2*/ 0, 0);

        car.GetComponent<CarController>().speedMultiplier = genes[5];
    }

    public float NumberMap(float input, float inputStartRange, float inputEndRange, float outputStartRange, float outputEndRange) {

        float output = (input - inputStartRange) / (inputEndRange - inputStartRange) * (outputEndRange - outputStartRange) + outputStartRange;

        return output;
    }

    public float GeneRandomizer(float geneIndex) {
        float result = 0;
        switch (geneIndex) {
            case 0:
                result = NumberMap(Random.Range(0f, 1f), 0, 1, 1.5f, 3.5f);
                break;
            case 1:
                result = NumberMap(Random.Range(0f, 1f), 0, 1, .25f, .75f);
                break;
            case 2:
                result = NumberMap(Random.Range(0f, 1f), 0, 1, .25f, .75f);
                break;
            case 3:
                result = NumberMap(Random.Range(0f, 1f), 0, 1, .1f, .9f);
                break;
            case 4:
                result = NumberMap(Random.Range(0f, 1f), 0, 1, .1f, .9f);
                break;
            case 5:
                result = NumberMap(Random.Range(0f, 1f), 0, 1, .5f, 1.75f);
                break;
        }

        return result;
    }



}

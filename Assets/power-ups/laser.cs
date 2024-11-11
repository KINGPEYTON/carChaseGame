using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour
{
    public playerCar pCar;
    public main controller;
    public speedometer speedo;
    public Animator ani;

    public float targX;
    public float targY;
    public Vector3 startPos;
    public float startTimer;
    public float startTime;
    public bool inPos;

    public float rotTarget;
    public float rotSpeed;

    public int uses;
    public float fireRate;
    public float cooldown;

    public float turnSpeed;
    public float turnTarget;

    public cars carToDestroy;
    public bool inShot;
    public float shootTimer;

    public bool inCooldown;
    public float cooldownTimer;
    public ParticleSystem cooldownSmoke;

    public bool destroyed;
    public bool ImDone;

    public AudioClip laserShoot;
    public AudioClip laserAim;
    public AudioClip laserClick;
    public AudioClip laserDone;

    // Start is called before the first frame update
    void Start()
    {
        pCar = GameObject.Find("playerCar").GetComponent<playerCar>();
        controller = GameObject.Find("contoller").GetComponent<main>();
        speedo = GameObject.Find("Speedometer").GetComponent<speedometer>();
        ani = GetComponent<Animator>();

        startPos = transform.localPosition;
        pCar.carLaser = this;
        startTime = 1;
        cooldownSmoke.enableEmission = false;
        ani.Play("laser shoot", 0, 19);
        ani.speed = 1 / fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inPos)
        {
            Vector3 dis = new Vector3(targX - startPos.x, targY - startPos.y, startTime);
            transform.localPosition = calcPos(dis, startPos, startTimer, startTime);
            transform.eulerAngles = new Vector3(0, 0, getValueScale(startTimer, 0, startTime, 40));
            startTimer += Time.deltaTime;
            if (startTimer > startTime / 2)
            {
                GetComponent<SpriteRenderer>().sortingOrder = pCar.livery.sortingOrder + 1;
            }
            if (startTimer > startTime)
            {
                inPos = true;
                controller.laserOn = true;
                transform.localPosition = new Vector3(targX, targY, 1);
                transform.eulerAngles = new Vector3(0, 0, 40);
                rotTarget = 0;
                AudioSource.PlayClipAtPoint(laserClick, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
            }
        }
        else if (destroyed)
        {
            transform.position = transform.position - new Vector3(Time.deltaTime / 6 * controller.mph, 0, 0); //moves guard across the screen
            if (transform.position.x <= -13) //checks if its offscreen
            {
                Destroy(gameObject);
            }
        }
        else if (ImDone)
        {
            amDone();
        }
        else if(controller.playing)
        {
            if (inShot)
            {
                if (carToDestroy != null)
                {
                    startRotate(carToDestroy.transform.position);
                    shootTimer += Time.deltaTime / fireRate;

                    if (shootTimer > 1)
                    {
                        shoot();
                    }
                }
                else
                {
                    inShot = false;
                }
            }
            else if (Input.touchCount > 0 && Time.timeScale > 0 && !inCooldown) // if the user touches the phone screen
            {
                Vector3 tapPoint = Camera.main.ScreenToWorldPoint(Input.touches[0].position); //calculates where the player taps on the screen
                startRotate(tapPoint);
            }

            if (inCooldown)
            {
                cooldownTimer -= Time.deltaTime;
                if(cooldownTimer < 0)
                {
                    inCooldown = false;
                    cooldownSmoke.enableEmission = false;
                }
            }
            else
            {
                if (transform.eulerAngles.z != rotTarget)
                {
                    rotateGun();
                }
            }
        }
    }

    public void startLaser(int shots, float fRate, float cdown)
    {
        uses = shots;
        fireRate = fRate;
        cooldown = cdown;
    }

    public void setTargetPos(float x, float y)
    {
        targX = x;
        targY = y;
    }

    public void getRideOfMe()
    {
        destroyed = true;
        controller.laserOn = false;
        transform.eulerAngles = new Vector3(0, 0, -40);
        AudioSource.PlayClipAtPoint(laserDone, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
    }

    public void makeTarget(cars c)
    {
        carToDestroy = c;
        inShot = true;
        shootTimer = 0;
        ani.Play("laser shoot", 0, 0);
        AudioSource.PlayClipAtPoint(laserAim, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
    }

    void rotateGun()
    {
        getRotSpeed();
        float transformAngle = transform.eulerAngles.z;
        if (transformAngle > 180) { transformAngle -= 360; }

        if (rotTarget != transform.eulerAngles.z)
        {
            transform.Rotate(0, 0, Time.deltaTime * rotSpeed);
        }
        if(Mathf.Abs(transformAngle - rotTarget) < 1.25f)
        {
            transform.eulerAngles = new Vector3(0, 0, rotTarget);
        }
    }

    void startRotate(Vector3 target)
    {
        float delta_x = target.x - transform.position.x;
        float delta_y = target.y - transform.position.y;
        float theta_radians = Mathf.Atan2(delta_y, delta_x) * 180.0f / Mathf.PI;

        if (theta_radians > 40) { rotTarget = 40; }
        else if (theta_radians < -40) { rotTarget = -40; }
        else { rotTarget = theta_radians; }
    }

    void getRotSpeed()
    {
        float transformAngle = transform.eulerAngles.z;
        if(transformAngle > 180) { transformAngle -= 360; }
        if (transformAngle < rotTarget) { rotSpeed = 60; }
        else if (transformAngle > rotTarget) {  rotSpeed = -60; }
    }

    void amDone()
    {
        ImDone = true;
        rotTarget = -50;
        float transformAngle = transform.eulerAngles.z;
        if (transformAngle > 180) { transformAngle -= 360; }
        if (transformAngle != rotTarget)
        {
            rotateGun();
        }
        else
        {
            getRideOfMe();
        }
    }

    void shoot()
    {
        carToDestroy.makeDestroyed();
        inShot = false;
        inCooldown = true;
        cooldownTimer = cooldown;
        cooldownSmoke.enableEmission = true;
        uses--;
        speedo.usePowerUp(1);
        AudioSource.PlayClipAtPoint(laserShoot, new Vector3(0, 0, -10), controller.masterVol * controller.sfxVol);
        if (uses <= 0)
        {
            amDone();
        }
    }

    Vector3 calcPos(Vector3 dis, Vector3 startScale, float targetTimer, float targetTime)
    {
        float xVal = getValueScale(targetTimer, 0, targetTime, dis.x);
        float yVal = getValueScale(targetTimer, 0, targetTime, dis.y) + ((targetTime * 2) - Mathf.Abs((targetTimer * 4) - (targetTime * 2)));
        return new Vector3(xVal, yVal, 0) + startScale;
    }

    float getValueScale(float val, float min, float max, float scale)
    {
        return (val / ((max - min) / scale)) - (min / ((max - min) / scale));
    }
}

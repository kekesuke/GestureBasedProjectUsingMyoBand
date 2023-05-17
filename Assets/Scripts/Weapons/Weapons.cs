using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pose = Thalmic.Myo.Pose;
using TMPro;

public class Weapons : MonoBehaviour
{


    public GameObject bullet;
    [SerializeField] private AudioSource[] audioSources;
    public float shootForce, upwardForce;

    public float timeBetweenShooting, spread, reloadTime, timeBetweenShoots;

    public int bulletspermagazine, magazineSize, bulletsPerTap;

    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;

    bool shooting, readyToShoot, reloading;

    public Camera fpsCamera;
    public Transform attackPoint;
    public GameObject muzzleFlash;

    private GameObject muzzleFlashCopy;
    public TextMeshProUGUI ammunitionDisplay;

    public Rigidbody playerRb;
    public float recoilForce;


    public bool allowInvoke = true;

    private bool noBullets;


    private void Awake()
    {
        bulletsLeft = bulletspermagazine;
        magazineSize -= bulletspermagazine;
        readyToShoot = true;
        myo = myoGameObject.GetComponent<ThalmicMyo>();
        Time.timeScale = 0;

    }

    private void Update()
    {
        MyInput();
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);

    }

    private void MyInput()
    {
        if (noBullets) return;

        if (magazineSize == 0 && bulletsLeft == 0 && !reloading)
        {
            UIController ui = FindObjectOfType<UIController>();
            ui.infoText.text = "No bullets left";
            StartCoroutine(ui.DisplayText(1f));
            noBullets = true;
        }
        //check if allowed to hold button
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
            Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0 && magazineSize > 0)
            Reload();


        //shooting if ready to shoot and not reloading and there are bullets left
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            Shoot();
        }
    }

    private void ShootingStart()
    {


        shooting = true;

        Invoke("StopShooting", 2f);

    }

    private void StopShooting()
    {
        shooting = false;
    }
    private void Shoot()
    {
        readyToShoot = false;

        Ray ray = fpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75);
        }

        //direction without spread
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;
        //spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        //direction with spread
        Vector3 directionWithSpread = directionWithoutSpread - new Vector3(x, y, 0);

        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);

        //rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        //add force to the bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        //if we want upward force
        //currentBullet.GetComponent<Rigidbody>().AddForce(fpsCamera.transform.up * upwardForce, ForceMode.Impulse);



        if (muzzleFlash != null)
        {

            Destroy(Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity), 1);
        }


        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
            playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
        }

        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShooting);
        }
        if (audioSources[0].isPlaying)
            return;
        else
            audioSources[0].Play();
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        magazineSize -= bulletspermagazine;
        UIController ui = FindObjectOfType<UIController>();
        ui.infoText.text = "Reloading";
        StartCoroutine(ui.DisplayText(reloadTime));
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = bulletspermagazine;
        reloading = false;
    }
}

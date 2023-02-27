using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Animations;


public class EnemyController : MonoBehaviour
{

    public Rigidbody shipRB;
    public string enemyShipName;
    public float hP;
    public float enemydamage;
    public int partstoDrop;
    public float gunRange;
    public float creditsToDrop;
    public GameObject player;
    public float distanceToPlayer;
    public TextMeshProUGUI distance;
    public TextMeshProUGUI enemyShipNameText;
    public TextMeshProUGUI indieDistance;
    public TextMeshProUGUI proxyHP;
    public float distanceToEngagePlayer;
    public float distanceToEvade;
    public ShipStatusAndHealthControl sshc;

    public bool playerInArea;
    public bool evade;
    public float shipSpeed;
    public float shipturn;
    public float shootCheckTimer;
    public Transform objecttofollow;

    public Transform weapon1ShootPoint;
    public bool firing;
    public float rateofFire;
    public float rateofFireTime;
    public GameObject weapon1Impact;
    public GameObject weapon1MuzzleFlash;
    public GameObject weapon1MuzzlePoint;
    public GameObject wreckmesh;

    public Transform camera1;
    public GameObject IndicatorforCameraPivot;
    public GameObject offscreenindicator;
    private Transform placetoSpawnIndicator;
    public Transform placetomoveoffscreenindie;
    public Transform placeToPutDamageIndicator;
    public string name2;
    public GameObject damageIndicator;

    public float scaleofFlash;
    public float scaleofImpact;
    public TurretEnemy tE;

    public float timerforreaction;

    public GameObject[] colliders;
    public GameObject enemyTransformArea;

    public Color alerted;
    public Color dead;
    public Image healthbar;
    public Image proxyUIHPbar;
    public Color unAlerted;
    public float starthp;
    public AudioSource aS;
    public GameObject proximityIndicator;
    public GameObject enemyProximityPanel;
    public GameObject damageIndicatorRed;
    public bool collidingEvade;
    public bool hasExploded;
    public bool doesExpode;
    public GameObject explosionPrefab;
    public GameObject impactMark;
    public float impactMarkScale;
    public GameObject thrusters;
  //  public GameObject partsdropindicator;
 //   public TextMeshProUGUI partsamountText;
    // Start is called before the first frame update
    void Start()
    {
       // partsdropindicator.SetActive(false);
        starthp = hP;
        name2 = IndicatorforCameraPivot.name;
        player = GameObject.Find("IcarusRB");
        camera1 = GameObject.Find("ShipCamera").transform;
        placetomoveoffscreenindie = GameObject.Find("MainCanvasPanel").transform;
        placetoSpawnIndicator = camera1;
        offscreenindicator.transform.SetParent(placetomoveoffscreenindie.transform);
        offscreenindicator.GetComponent<offScreenIndicator>().sceneObject = this.gameObject.transform;
        offscreenindicator.GetComponent<offScreenIndicator>().ShipCamera = camera1.GetComponent<Camera>();
        if(hP <= 0)
        {
            hasExploded = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (distanceToPlayer < distanceToEngagePlayer && hP > 0)
        {
            proximityIndicator.SetActive(true);
            //proxyHP.text = hP.ToString("F0");
        }
        if (distanceToPlayer > distanceToEngagePlayer || hP <= 0)
        {
            proximityIndicator.SetActive(false);
        }
        healthbar.fillAmount = hP / starthp;
        proxyUIHPbar.fillAmount = hP / starthp;
        if (distanceToPlayer < distanceToEngagePlayer)
        {
            IndicatorforCameraPivot.SetActive(true);
            offscreenindicator.SetActive(true);
        }
        if (distanceToPlayer > distanceToEngagePlayer)
        {
            IndicatorforCameraPivot.SetActive(false);
            offscreenindicator.SetActive(false);
        }




        IndicatorforCameraPivot.transform.SetParent(placetoSpawnIndicator);
        this.transform.localScale = new Vector3(1, 1, 1);
        if (!camera1.GetComponent<Camera>().enabled)
        {
            offscreenindicator.GetComponent<offScreenIndicator>().indictaor.enabled = false;
            offscreenindicator.GetComponent<offScreenIndicator>().textdis.enabled = false;

        }
        if (camera1.GetComponent<Camera>().enabled)
        {
            offscreenindicator.GetComponent<offScreenIndicator>().enabled = true;
        }
        if (hP > 0)
        {
            thrusters.SetActive(true);
            RaycastHit hit2;
            if (Physics.Raycast(weapon1ShootPoint.position, weapon1ShootPoint.forward, out hit2, 150f))
            {
                if (hit2.transform.CompareTag("Roid"))
                {

                    collidingEvade = true;

                }

            }
            else
            {
                collidingEvade = false;
            }

            if (distanceToPlayer > distanceToEngagePlayer)
            {
                enemyShipNameText.color = unAlerted;
                distance.color = unAlerted;
            }
            if (distanceToPlayer < distanceToEngagePlayer)
            {
                enemyShipNameText.color = alerted;
                distance.color = alerted;
            }
            //shipRB.isKinematic = false;
            IndicatorforCameraPivot.transform.position = camera1.position;
            IndicatorforCameraPivot.transform.LookAt(this.transform, player.transform.up);
            distance.text = distanceToPlayer.ToString("F0");
            enemyShipNameText.text = enemyShipName;
            transform.gameObject.layer = 3;
            objecttofollow.LookAt(player.transform);

            indieDistance.text = distanceToPlayer.ToString("F0");

            foreach (GameObject mesh in colliders)
            {
                mesh.transform.parent = this.transform;
                mesh.transform.GetComponent<MeshCollider>().convex = true;
            }

        }
        if (hP <= 0)
        {
            thrusters.SetActive(false);
            offscreenindicator.SetActive(false);
            aS.Stop();
            foreach (GameObject mesh in colliders)
            {
                mesh.transform.parent = enemyTransformArea.transform;
                mesh.transform.GetComponent<MeshCollider>().convex = false;
            }
            //shipRB.isKinematic = true;
            indieDistance.text = distanceToPlayer.ToString("F0");
            IndicatorforCameraPivot.transform.LookAt(this.transform, player.transform.up);
            distance.text = "";
            enemyShipNameText.text = enemyShipName;

            tE.enabled = false;
            transform.gameObject.layer = 3;
            IndicatorforCameraPivot.transform.position = camera1.position;
            enemyShipNameText.color = dead;
            distance.color = dead;
             if (doesExpode && !hasExploded)
             {
               // partsamountText.text = partstoDrop.ToString();
               // partsdropindicator.transform.SetParent(placetomoveoffscreenindie);
             //   partsdropindicator.SetActive(true);
                hasExploded = true;
                GameObject explosion;
                explosion = Instantiate(explosionPrefab, transform);
                sshc.parts += partstoDrop;
                Destroy(explosion, 10f);

             }


        }
        if (hP > 0)
        {
            distance.text = distanceToPlayer.ToString("F0");
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            enemyShipNameText.text = enemyShipName;
            if (distanceToPlayer < distanceToEngagePlayer)
            {
                timerforreaction += Time.deltaTime;
                if (timerforreaction > 0.2f)
                {

                    playerInArea = true;
                    shootCheckTimer += Time.deltaTime;
                }
            }

            if (distanceToPlayer < distanceToEvade)
            {

                evade = true;
                playerInArea = false;

            }


            if (distanceToPlayer > distanceToEngagePlayer)
            {
                playerInArea = false;
                evade = false;
            }


            if (Input.GetKey(KeyCode.Space))
            {
                firing = true;
            }
            if (!Input.GetKey(KeyCode.Space))
            {
                firing = false;
            }


            if (playerInArea)
            {
                rateofFire += Time.deltaTime;

                if (playerInArea && rateofFire >= rateofFireTime)
                {
                    RaycastHit hit1;
                    if (Physics.Raycast(weapon1ShootPoint.position, weapon1ShootPoint.forward, out hit1, gunRange))
                    {

                        rateofFire = 0;
                        if (hit1.transform.CompareTag("Player"))
                        {
                            GameObject impactmarkleft;
                            impactmarkleft = Instantiate(impactMark, hit1.transform);
                            impactmarkleft.transform.position = hit1.point;
                            impactmarkleft.transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hit1.normal);
                            impactmarkleft.transform.localScale = new Vector3(impactMarkScale, impactMarkScale, impactMarkScale);
                            Destroy(impactmarkleft, 2f);
                            GameObject damageObject;
                            damageObject = Instantiate(damageIndicatorRed, placeToPutDamageIndicator);
                            damageObject.transform.SetAsFirstSibling();
                            // Debug.Log("Play/Add mussleflash sound and tracer effect here");
                            GameObject decalObject = Instantiate(weapon1Impact, hit1.point + (hit1.normal * 0.025f), Quaternion.identity) as GameObject;
                            decalObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit1.normal);
                            decalObject.transform.parent = hit1.transform;
                            decalObject.transform.localScale = new Vector3(scaleofImpact, scaleofImpact, scaleofImpact);
                            //
                            GameObject muzzleflash = Instantiate(weapon1MuzzleFlash, weapon1MuzzlePoint.transform.position, weapon1MuzzlePoint.transform.rotation);
                            muzzleflash.transform.parent = weapon1MuzzlePoint.transform;
                            muzzleflash.transform.localScale = new Vector3(scaleofFlash, scaleofFlash, scaleofFlash);
                            //
                            sshc.hullHP -= enemydamage;
                            sshc.o2HP -= enemydamage / 4;
                            sshc.engineHP -= enemydamage / 2;


                        }

                        if (hit1.transform.CompareTag("Thursters"))
                        {
                            GameObject impactmarkleft;
                            impactmarkleft = Instantiate(impactMark, hit1.transform);
                            impactmarkleft.transform.position = hit1.point;
                            impactmarkleft.transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hit1.normal);
                            impactmarkleft.transform.localScale = new Vector3(impactMarkScale, impactMarkScale, impactMarkScale);
                            Destroy(impactmarkleft, 4f);
                            GameObject damageObject;
                            damageObject = Instantiate(damageIndicatorRed, placeToPutDamageIndicator);
                            damageObject.transform.SetAsFirstSibling();
                            // Debug.Log("Play/Add mussleflash sound and tracer effect here");
                            GameObject decalObject = Instantiate(weapon1Impact, hit1.point + (hit1.normal * 0.025f), Quaternion.identity) as GameObject;
                            decalObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit1.normal);
                            decalObject.transform.parent = hit1.transform;
                            decalObject.transform.localScale = new Vector3(scaleofImpact, scaleofImpact, scaleofImpact);
                            //
                            GameObject muzzleflash = Instantiate(weapon1MuzzleFlash, weapon1MuzzlePoint.transform.position, weapon1MuzzlePoint.transform.rotation);
                            muzzleflash.transform.parent = weapon1MuzzlePoint.transform;
                            muzzleflash.transform.localScale = new Vector3(scaleofFlash, scaleofFlash, scaleofFlash);
                            //
                            sshc.hullHP -= enemydamage / 4;
                            sshc.o2HP -= enemydamage / 4;
                            sshc.engineHP -= enemydamage;


                        }

                        if (hit1.transform.CompareTag("weaponChild"))
                        {
                            GameObject impactmarkleft;
                            impactmarkleft = Instantiate(impactMark, hit1.transform);
                            impactmarkleft.transform.position = hit1.point;
                            impactmarkleft.transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hit1.normal);
                            impactmarkleft.transform.localScale = new Vector3(impactMarkScale, impactMarkScale, impactMarkScale);
                            Destroy(impactmarkleft, 4f);
                            GameObject damageObject;
                            damageObject = Instantiate(damageIndicatorRed, placeToPutDamageIndicator);
                            damageObject.transform.SetAsFirstSibling();
                            // Debug.Log("Play/Add mussleflash sound and tracer effect here");
                            GameObject decalObject = Instantiate(weapon1Impact, hit1.point + (hit1.normal * 0.025f), Quaternion.identity) as GameObject;
                            decalObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit1.normal);
                            decalObject.transform.parent = hit1.transform;
                            decalObject.transform.localScale = new Vector3(scaleofImpact, scaleofImpact, scaleofImpact);
                            //
                            GameObject muzzleflash = Instantiate(weapon1MuzzleFlash, weapon1MuzzlePoint.transform.position, weapon1MuzzlePoint.transform.rotation);
                            muzzleflash.transform.parent = weapon1MuzzlePoint.transform;
                            muzzleflash.transform.localScale = new Vector3(scaleofFlash, scaleofFlash, scaleofFlash);
                            //
                            sshc.hullHP -= enemydamage;
                            //
                            if (hit1.transform.parent.GetComponent<ShootingShipWeaponsScript>() != null)
                            {
                                hit1.transform.parent.GetComponent<ShootingShipWeaponsScript>().hp -= enemydamage;
                            }



                        }
                        if (hit1.transform.CompareTag("weaponChildChild"))
                        {
                            GameObject impactmarkleft;
                            impactmarkleft = Instantiate(impactMark, hit1.transform);
                            impactmarkleft.transform.position = hit1.point;
                            impactmarkleft.transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hit1.normal);
                            impactmarkleft.transform.localScale = new Vector3(impactMarkScale, impactMarkScale, impactMarkScale);
                            Destroy(impactmarkleft, 4f);
                            GameObject damageObject;
                            damageObject = Instantiate(damageIndicatorRed, placeToPutDamageIndicator);
                            damageObject.transform.SetAsFirstSibling();
                            // Debug.Log("Play/Add mussleflash sound and tracer effect here");
                            GameObject decalObject = Instantiate(weapon1Impact, hit1.point + (hit1.normal * 0.025f), Quaternion.identity) as GameObject;
                            decalObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit1.normal);
                            decalObject.transform.parent = hit1.transform;
                            decalObject.transform.localScale = new Vector3(scaleofImpact, scaleofImpact, scaleofImpact);
                            //
                            GameObject muzzleflash = Instantiate(weapon1MuzzleFlash, weapon1MuzzlePoint.transform.position, weapon1MuzzlePoint.transform.rotation);
                            muzzleflash.transform.parent = weapon1MuzzlePoint.transform;
                            muzzleflash.transform.localScale = new Vector3(scaleofFlash, scaleofFlash, scaleofFlash);
                            //
                            if (hit1.transform.parent.parent.GetComponent<ShootingShipWeaponsScript>() != null)
                            {
                                hit1.transform.parent.parent.GetComponent<ShootingShipWeaponsScript>().hp -= enemydamage;
                            }



                        }

                    }
                }
            }

        }


    }


    private void FixedUpdate()
    {
        if (hP > 0)
        {
            if (!aS.isPlaying)
            {
                aS.Play();
            }
            if (playerInArea == false && !collidingEvade)
            {

                shipRB.AddForce(transform.forward * shipSpeed / 18f);
                shipRB.transform.rotation = Quaternion.Lerp(transform.rotation, objecttofollow.transform.rotation, Time.deltaTime * shipturn);
            }
            if (playerInArea == true && !collidingEvade)
            {
                var heading = player.transform.position - transform.position;

                shipRB.AddForce(transform.forward * shipSpeed);
                shipRB.transform.rotation = Quaternion.Lerp(transform.rotation, objecttofollow.transform.rotation, Time.deltaTime * shipturn * 20f);
                if (shootCheckTimer > 0.1f)
                {
                    RaycastHit hit;
                    if (Physics.SphereCast(transform.position, 5, transform.forward, out hit, distanceToEngagePlayer))
                    {
                        //Debug.Log(hit.transform.name);
                        timerforreaction = 0;
                    }
                    shootCheckTimer = 0;
                }
            }

            if (collidingEvade)
            {
                shipRB.AddForce(-transform.forward * shipSpeed * 2);
                shipRB.AddRelativeTorque(-Vector3.up * shipSpeed);
            }

            if (evade && !collidingEvade)
            {
                shipRB.AddForce(transform.forward * shipSpeed);
                shipRB.AddRelativeTorque(-Vector3.forward * Random.Range(-shipSpeed / 2, shipSpeed / 2));
                shipRB.AddRelativeTorque(-Vector3.right * Random.Range(-shipSpeed, shipSpeed));
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceToEngagePlayer);
        Gizmos.DrawWireSphere(transform.position, distanceToEvade);

    }

    public void OnCollisionEnter(Collision collision)
    {

        hP -= 38f;

    }
    
}

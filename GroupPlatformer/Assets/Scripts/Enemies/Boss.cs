using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : AdvancedEnemy
{

    private Vector3 spawnPosition;


    private float baseNormalSpeed;
    [SerializeField]
    private float timeSincePlayerSeen;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int health;
    private bool invincible;
    [SerializeField]
    private float stunTime;
    public GameObject explosionPrefab;
    [SerializeField]
    private GameObject healthBar;
    new public enum Behaviour{
        
        ChargeAndDestroy,
        SearchAndSpawn,
        PatrolAndShoot, 
        Stunned


    }
    [SerializeField]
    private Behaviour bossBehaviour;
    private bool spawning;
    private bool charging;
    [SerializeField]
    private float chargeSpeedUp;
    [SerializeField]
    private float liftoffSpeed;
    [SerializeField]
    private float liftoffHeight;
    [SerializeField]
    private GameObject enemyPrefab;
    private bool shooting;
    [SerializeField]
    private GameObject dartPrefab;
    private float baseWeaponCooldown;
    [SerializeField]
    private float weaponCooldown;
    [SerializeField]
    private float rotationSpeed;


    new void Awake(){
        base.Awake();
        liftoffHeight += this.transform.position.y;
        invincible = spawning = shooting = false;
        healthBar.transform.parent.GetComponent<Image>().enabled = true;
        healthBar.GetComponent<Image>().enabled = true;
        timeSincePlayerSeen = 0;
        bossBehaviour = Behaviour.ChargeAndDestroy;
        spawnPosition = transform.position;
    }

    void Start(){
        maxHealth = health;    
        baseNormalSpeed = normalSpeed;
        baseWeaponCooldown = weaponCooldown;
    }
    private void FixedUpdate(){
        switch(bossBehaviour) {
            case Behaviour.ChargeAndDestroy : 
                if (!PlayerVisible(prey.transform.position)){
                    normalSpeed = baseNormalSpeed;
                    PatternMovement();
                }
                else {
                    charging = true;
                    Charge(prey.transform.position);
                }

                break;
            case Behaviour.PatrolAndShoot : 

                    if (PlayerVisible(prey.transform.position)){
                        
                        timeSincePlayerSeen = 0;
                        weaponCooldown -= Time.deltaTime;
                        if (Vector3.Dot(transform.forward, (prey.transform.position - transform.position).normalized) > 0.99f){

                            if(weaponCooldown <= 0){
                                
                                Instantiate(dartPrefab, this.transform.position, this.transform.rotation); 
                                weaponCooldown = baseWeaponCooldown;
                            }

                        }
                        
                        else{
                                Vector3 target = prey.transform.position - transform.position;
                                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, target, 1f*Time.deltaTime, 10f));

                        }
                    }
                    else{
                        PatternMovement();
                        timeSincePlayerSeen += Time.deltaTime;
                        if (timeSincePlayerSeen > 4 && !shooting){
                            StartCoroutine(ShootBlindly());
                            timeSincePlayerSeen = 0;
                        }
                    }
                    break;
            case Behaviour.SearchAndSpawn:
                if(!spawning){
                    if (timeSincePlayerSeen > 5 &&!PlayerVisible(prey.transform.position)){
                        timeSincePlayerSeen = 0;
                        StartCoroutine(Spawn());
                    }
                    else if (PlayerVisible(prey.transform.position)){
                        timeSincePlayerSeen = 0;
                        ChaseLineOfSight(prey.transform.position, chaseSpeed);
                    }
                    else{
                        
                        timeSincePlayerSeen += Time.deltaTime;
                        PatternMovement();
                    }
                }
                break;


                
        }
    }

    public void OnHit(int hpLoss){
        if (!invincible){
        health -= hpLoss;
        if (health < 2*maxHealth / 3 && bossBehaviour != Behaviour.SearchAndSpawn && bossBehaviour != Behaviour.PatrolAndShoot){
            normalSpeed = baseNormalSpeed;
            charging = false;
            StartCoroutine(SwitchPhase(Behaviour.SearchAndSpawn));
        }
        else if (health < maxHealth / 3 && bossBehaviour != Behaviour.PatrolAndShoot){
            StartCoroutine(SwitchPhase(Behaviour.PatrolAndShoot));
        }
        if (health <= 0){
            OnDeath();
        }
        invincible = true;
        UpdateHealthbar();
        StartCoroutine(Hit());
        }
    }

    private IEnumerator SwitchPhase(Behaviour phase){
        timeSincePlayerSeen = 0;
        Color newColor;
        float colorChange = 0;
        bossBehaviour = Behaviour.Stunned;
        while (colorChange < 1){
            colorChange += Time.deltaTime/2;
            if (phase == Behaviour.SearchAndSpawn)
                newColor = Color.Lerp(Color.blue, Color.red, colorChange);
            else 
                newColor = Color.Lerp(Color.red, Color.black, colorChange);
            this.gameObject.GetComponent<Renderer>().material.color = newColor;
            yield return null;
        }
        bossBehaviour = phase;

    }

    new private void OnDeath(){
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(healthBar.transform.GetComponentInParent<Image>().gameObject);
        Destroy(gameObject,1.0f);
        GameObject.Find("WinText").GetComponent<Text>().enabled = true;
    }

    IEnumerator Hit(){
        for (int i = 0; i < 5; i++){
            this.gameObject.GetComponent<Renderer>().enabled = false;
            yield return new WaitForSeconds(0.1f);
            this.gameObject.GetComponent<Renderer>().enabled = true;
            yield return new WaitForSeconds(0.2f);
            }
        invincible = false;
    }

    private void UpdateHealthbar(){
        healthBar.transform.localScale = new Vector3 ((health*1.0f)/(maxHealth*1.0f), healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }

    private void Charge(Vector3 targetPosition){
        Vector3 enemyPosition = transform.position;
        Vector3 velocityRelative, distance, predictedInterceptPoint;
        float timeToClose;

        velocityRelative = prey.GetComponent<Rigidbody>().velocity - enemyRigidbody.velocity;
        distance = targetPosition - enemyPosition;

        timeToClose = distance.magnitude / velocityRelative.magnitude;

        predictedInterceptPoint = targetPosition + (timeToClose* prey.GetComponent<Rigidbody>().velocity);
        if (PlayerVisible(prey.transform.position)) 
            normalSpeed += chargeSpeedUp*Time.deltaTime; 
        else if (normalSpeed > baseNormalSpeed)
            normalSpeed -= chargeSpeedUp*Time.deltaTime;
        ChaseLineOfSight(predictedInterceptPoint, normalSpeed);
    }

    private void OnCollisionEnter(Collision collision){
        if (charging && normalSpeed > baseNormalSpeed * 1.2f){
            if (collision.gameObject.CompareTag("Breakable Wall")){
                charging = false;
                OnHit(5);
                DestroyWall(collision.gameObject);
                StartCoroutine(Concussion());
            }
            if (collision.gameObject.CompareTag("Wall")){
                charging = false;
                OnHit(5);
                StartCoroutine(Concussion());
            }
        charging = false;
        normalSpeed = baseNormalSpeed;
        }
    }

    private void DestroyWall(GameObject wall){
        wall.GetComponent<Rigidbody>().isKinematic = false;
        wall.GetComponent<Rigidbody>().useGravity = true;
        wall.GetComponent<Collider>().enabled = false;
        Destroy(wall, 1f);

    }

    private IEnumerator Spawn(){
        spawning = true;
        Rigidbody rigidbody = this.GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;
        while(this.transform.position.y < liftoffHeight){
                this.transform.position += Vector3.up*Time.deltaTime*liftoffSpeed;
                yield return null;
            }
        float spawned = 0;
        while(spawned < 3){
            GameObject minion = Instantiate(enemyPrefab, this.transform.position + this.GetComponent<Collider>().bounds.extents.y * Vector3.down, Quaternion.identity);
            AdvancedEnemy spawnedMinion = minion.GetComponent<AdvancedEnemy>();
            spawnedMinion.prey = this.prey;
            health -= 1;
            if (health < 2*maxHealth / 3 && bossBehaviour != Behaviour.SearchAndSpawn){
                normalSpeed = baseNormalSpeed;
                charging = false;
                StartCoroutine(SwitchPhase(Behaviour.SearchAndSpawn));
                }
            else if (health < maxHealth / 3 && bossBehaviour != Behaviour.PatrolAndShoot){
                StartCoroutine(SwitchPhase(Behaviour.PatrolAndShoot));
            }
            if (health <= 0){
                OnDeath();
            }
            UpdateHealthbar();
            spawned++;
            yield return new WaitForSeconds(2);
        }
        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;
        yield return new WaitForSeconds(0.3f);
        spawning = false;


    }



    private IEnumerator Concussion(){
        Behaviour previousBehaviour = bossBehaviour;
        bossBehaviour = Behaviour.Stunned;
        yield return new WaitForSeconds(stunTime);
        bossBehaviour = previousBehaviour; 
    }

    private IEnumerator ShootBlindly(){
        shooting = true;
        for (int dartsFired = 0; dartsFired < 36&& !PlayerVisible(prey.transform.position); dartsFired++){
                Instantiate(dartPrefab, this.transform.position + this.gameObject.GetComponent<Collider>().bounds.extents.x * Vector3.forward, this.transform.rotation); 
                for (float rotationAroundSelf = 0; rotationAroundSelf < 10 && !PlayerVisible(prey.transform.position); rotationAroundSelf += Time.deltaTime*rotationSpeed){
                    this.transform.Rotate(0, Time.deltaTime*rotationSpeed, 0, Space.Self);
                    yield return null;
                }
                yield return null;
        }
        shooting = false;
    }

    public void SetToSpawn(){
        this.transform.position = spawnPosition;
    }
        
}



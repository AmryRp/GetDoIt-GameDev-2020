
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PlayerController : Player, ISinkable, IDrainable<float>, IMoveable<float>, IStopable<float>
{
    public PlayerController(string name, int point, int movespeed) : base(name, point, movespeed)
    {

    }

    private static PlayerController instance;
    public static PlayerController MyPlayerControl
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerController>();
            }
            return instance;
        }
    }
    protected override void Start()
    {
       
        SetDefault();
        base.Start();
    }
    //load data tersimpan dr equip
    private void PlayerPrefsLoads()
    {//for character
     //MoveSpeed = PlayerPrefs.GetFloat("MySpeed");
     //EnergyDrain = PlayerPrefs.GetFloat("MyFatigue");
     //PlayerEnergy = PlayerPrefs.GetFloat("MaxEnergy");
     //for CanoeType
        float str1 = PlayerPrefs.GetFloat("MyEquipSpeed");
        float str2 = PlayerPrefs.GetFloat("MyEquipWeight");
        float str3 = PlayerPrefs.GetFloat("MyEquipPoint");
        if (!PlayerPrefs.GetFloat("MyEquipSpeed").Equals(0)  && !PlayerPrefs.GetFloat("MyEquipWeight").Equals(0)
            && !PlayerPrefs.GetFloat("MyEquipPoint").Equals(0) )
        {
            int intC = PlayerPrefs.GetInt("CanoeType");
            if (!intC.Equals(-1))
            {
                CanoeTypeUsed.GetComponent<SpriteRenderer>().sprite = ShoppingListManager.MyInstance.CanoeImageStatic[intC];
            }
            else
            {
                CanoeType = -1;
                CanoeTypeUsed.GetComponent<SpriteRenderer>().sprite = null;
            }
            //print(CanoeTypeUsed);
            loadDataPdj();
            //print("Speed : " + canoeSpeed + "\n Weight : " + canoeWeight + "\nPoint : " + canoePoint);
        }
        else
        {
            CanoeType = -1;
            CanoeTypeUsed.GetComponent<SpriteRenderer>().sprite = null;
            PlayerPrefs.SetFloat("MyEquipSpeed", 1f);
            PlayerPrefs.SetFloat("MyEquipWeight", 1f);
            PlayerPrefs.SetFloat("MyEquipPoint", 1f);
            PlayerPrefs.Save();
            loadDataPdj();
        }
    }
    public void loadDataPdj()
    {
        canoeSpeed = PlayerPrefs.GetFloat("MyEquipSpeed");
        canoeWeight = PlayerPrefs.GetFloat("MyEquipWeight");
        canoePoint = PlayerPrefs.GetFloat("MyEquipPoint");
        
    }
    public void SetDefault()
    {
        ObjectivesManager.MyInstance.LoadObjectives();
        PlayerPrefsLoads();
        lastMove = transform.position;
        CanoeBody = GetComponent<Rigidbody2D>();
        TapCount = 0;
        DistanceTraveled = GameObject.FindGameObjectWithTag("T_Distance").GetComponent<Text>();
        Energy = GameObject.FindGameObjectWithTag("bar energy").GetComponent<Status>();
        Energy.Initialize(PlayerEnergy, PlayerEnergy);
        //MyExp.Initialize(0, Mathf.Floor(100 * myLevel * Mathf.Pow(myLevel, 0.5f)));
        //LevelText.text = myLevel.ToString();
    }
    protected override void Update()
    {
        //MyScene = SceneManager.GetActiveScene().buildIndex;
        //untuk handle animasi
        //HandleLayers();
        //untuk mobile touch 
        GM = GameManager.MyGM;
        if (!GM.IsPaused && Hidup)
        {
            TouchScreen();
            DistanceTravel();
            StartCoroutine(DrainIt());
            CameraObjectManager.MyCamReceiver.CalculateMeter();
        }
        else if (!Hidup)
        {
            GM.isDeath = true;
        }
        if (Energy.MyCurrentValue <= Energy.MyMaxValue * 0.3f)
        {
            LowHP.SetBool("LowHP", true);
        }
        else
        {
            LowHP.SetBool("LowHP", false);
        }
        //untuk drain energy
        //Drain();
        base.Update();
        //Untuk Test DiUnity
        //#if UNITY_EDITOR

        //        UnityEditorMovement();
        //#endif
    }

    private void TouchScreen()
    {
        if (!IsMoving)
        {
            IsAnimator.SetBool(IDLE, true);
            if (IsAnimator.GetBool(IDLE) && !IsAnimator.GetBool(MOVING))
            {

                IdleTimer += Time.deltaTime;
                // print(IdleTimer);
                if (IdleTimer >= MaxTouchWait)
                {

                    multiplier = 1;
                    IsAnimator.SetFloat("MoveMultiplier", multiplier);
                    IdleTimer = 0f;
                    if (!playerTimeMode)
                    {
                        if (multiplier.Equals(1))
                        {
                            Animator StmUI = StaminaUI.GetComponent<Animator>();
                            StmUI.SetBool("LimitReached", false);
                            Attention.GetComponent<Image>().enabled = false;
                            Stamina.fillAmount = 0;
                            StaminaUI.GetComponent<Canvas>().enabled = false;
                        }
                    }
                }
            }
            else
            {
                IdleTimer = 0f;
            }
            IsAnimator.SetBool(MOVING, false);
            IsAnimator.SetBool(STOP, false);
            MoveBoatSplash.Stop();
        }

        if (Input.GetMouseButton(0) || Input.touchCount > 0 &&
            !(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)))
        {
            if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                one_click = true;
                timer_for_double_click = Time.time;
                //Debug.Log("Not Touched the UI");
                if (!is_hold)
                {
                    //movement pertama
                    if (!playerTimeMode)
                    {
                        StaminaUI.GetComponent<Canvas>().enabled = true;
                    }
                    IsAnimator.SetBool(STOP, false);
                    IsAnimator.SetBool(IDLE, false);
                    IsAnimator.SetBool(MOVING, true);
                    IsAnimator.SetTrigger("IsMove");
                    if (!speedLimiter(IsAnimator.GetFloat("MoveMultiplier")))
                    {
                        //print("jalan");
                        multiplier += 0.37f;
                        IsAnimator.SetFloat("MoveMultiplier", multiplier);
                        StartCoroutine(MovePlayer(MoveSpeedInWater));
                        if (!playerTimeMode)
                        {
                            Stamina.fillAmount = multiplier / maxmultiplier;
                        }
                    }
                    else
                    {
                        if (!playerTimeMode)
                        {
                            Animator StmUI = StaminaUI.GetComponent<Animator>();
                            StmUI.SetBool("LimitReached", true);
                            Attention.GetComponent<Image>().enabled = true;
                            TakeDamage(0.5f);
                        }
                        IsAnimator.SetFloat("MoveMultiplier", multiplier);
                        StartCoroutine(MovePlayer(MoveSpeedInWater));

                    }
                    //one_click = false;
                }

            }


            //if (Input.GetTouch(0).phase == TouchPhase.Ended)
            //{
            //    IsAnimator.SetBool(IDLE, true);
            //    IsAnimator.SetBool(MOVING, false);
            //    IsAnimator.SetBool(STOP, false);
            //    is_hold = false;
            //    acumTime = 0;
            //}
            //Double tap
            if (Input.GetMouseButton(0) || Input.touchCount == 1)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Ended)
                    {
                        is_hold = false;
                        TapCount += 1;
                    }

                    if (TapCount == 1)
                    {
                        NewTime = Time.time + MaxDubbleTapTime;
                        //jika terlalu lama hold / stop
                        if (one_click)
                        {
                            // if the time now is delay seconds more than when the first click started. 
                            if ((Time.time - timer_for_double_click) < delay)
                            {
                                //print("One Hold Time Safe");
                                //movement kedua
                                IsAnimator.SetBool(STOP, false);
                                IsAnimator.SetBool(IDLE, false);
                                IsAnimator.SetBool(MOVING, true);
                                IsAnimator.SetTrigger("IsMove");
                                if (!speedLimiter(IsAnimator.GetFloat("MoveMultiplier")))
                                {
                                    multiplier += 0.25f;
                                    IsAnimator.SetFloat("MoveMultiplier", multiplier);
                                    StartCoroutine(MovePlayer(MoveSpeedInWater));

                                }
                                else
                                {

                                    Stamina.fillAmount = multiplier / maxmultiplier;

                                }
                                is_hold = false;
                                one_click = false;
                            }
                        }
                        Hold_timer = 0f;

                    }
                    else if (TapCount == 2)
                    {

                        //Whatever you want after a dubble tap    
                        //print("Dubble tap");
                        TapCount = 0;
                    }
                }
            }

            //hold
            if (Input.GetMouseButton(1) || Input.touchCount == 2)
            {
                bool moveIt = false;
                if (Application.platform == RuntimePlatform.Android)
                {
                    if (Input.GetTouch(0).phase != TouchPhase.Ended)
                    {
                        moveIt = true;
                    }
                    else
                    {
                        moveIt = false;
                    }
                }
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        moveIt = true;
                    }
                    else 
                    {
                        moveIt = false;
                    }
                }
                if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        moveIt = true;
                    }
                    else
                    {
                        moveIt = false;
                    }
                }
                if (moveIt)
                {
                    
                    if (Application.platform == RuntimePlatform.Android)
                    {
                        acumTime += Input.GetTouch(0).deltaTime;
                    }
                    if (Application.platform == RuntimePlatform.WindowsPlayer)
                    {
                        acumTime += Time.deltaTime;
                        StartCoroutine(BreakPlayer(MoveSpeedInWater));
                    }
                    if (Application.platform == RuntimePlatform.WindowsEditor)
                    {
                        acumTime += Time.deltaTime;
                        StartCoroutine(BreakPlayer(MoveSpeedInWater));
                    }
                    if (acumTime >= holdTime)
                    {
                        //2 finger Long tap
                       

                        is_hold = true;
                        StartCoroutine(BreakPlayer(MoveSpeedInWater));

                        if ((Time.time - timer_for_double_click) > delay)
                        {
                            // print("One Hold Time Reached");
                            StartCoroutine(MovePlayer(0.6f));
                            is_hold = false;
                            one_click = false;
                            acumTime = 0f;
                            //IsAnimator.SetBool(STOP, false);
                            //IsAnimator.SetBool(MOVING, false);
                        }
                    }
                }
                else
                {
                    IsAnimator.SetBool(IDLE, true);
                    IsAnimator.SetBool(MOVING, false);
                    IsAnimator.SetBool(STOP, false);
                    is_hold = false;
                    acumTime = 0;
                }
            }
            if (Time.time > NewTime)
            {
                /*IsAnimator.SetBool(IDLE, true);
                IsAnimator.SetBool(MOVING, false);*/
                TapCount = 0;
                MoveBoatSplash.Stop();
            }

        }
    }
    public IEnumerator StartIdling()
    {
        float elapsedTime = 0;

        while (elapsedTime < 3f)
        {
            // print("Idling");
            IdleTimer += Time.deltaTime;
            elapsedTime += Time.deltaTime;
        }
        yield return null;
    }
    public IEnumerator MovePlayer(float moveSpeed)
    {
        // yield return new WaitForSeconds(0.1f * multiplier);
        //print("move");
        // Vector3 startingPos = transform.position;
        // Vector3 finalPos = transform.position + (transform.right * MoveSpeed);
        float elapsedTime = 0;
        // print("MOVE");
        float directMove = 1;
        //dikali canoe speed dan dibagi canoe weight nantinya
        if (MoveSpeedInWater <= 0)
        {
            directMove = 0.5f;
        }
        else
        {
            directMove = MoveSpeedInWater;
        }

        while (elapsedTime < MaxAnimTime)
        {

            //Untuk pergerakan
            isMoving = true;
            MoveBoatSplash.Play();

            //print((directMove + moveSpeed + ((canoeSpeed / canoeWeight) * 5)));
             //SSTools.ShowMessage(((directMove + moveSpeed + (((canoeSpeed / canoeWeight) * 2) + multiplier) / 3) / 2).ToString(), SSTools.Position.bottom, SSTools.Time.twoSecond);
            CanoeBody.AddForce(new Vector2(((directMove + moveSpeed + (((canoeSpeed / canoeWeight) * 2) + multiplier) / 3) / 2 * Time.deltaTime), 0), ForceMode2D.Impulse); // Movement
                                                                                                                                                                            //MovementSpeedInWater kecepatan canoe berdasarkan deras air atau bisa ditambah dengan moveSpeed kecepatan dari player, seperti dibawah ini
            /*CanoeBody.MovePosition(transform.position + transform.right * ((MoveSpeedInWater + moveSpeed) * Time.deltaTime)); */
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (elapsedTime >= MaxAnimTime)
        {
            yield break;
        }

        //IsAnimator.SetBool(IDLE, true);
        //IsAnimator.SetBool(MOVING, false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water")) // Ketika canoe bersentuhan dengan water
        {
            BuoyancyEffector2D effectWater;
            effectWater = collision.gameObject.GetComponent<BuoyancyEffector2D>();
            effectWater.flowMagnitude = DefaultWaterStream;
            MoveSpeedInWater = effectWater.flowMagnitude; //Set Canoe Water speed berdasarkan deras aliran air yang disentuh
            //Debug.Log("Touch with water");
        }
    }
    public IEnumerator DrainIt()
    {
        float elapsedTime = 0;
        while (elapsedTime < maxDrainSpeedTime && Hidup && !GM.IsPaused)
        {
            Drain();
            elapsedTime += Time.deltaTime;
            yield return new WaitForSecondsRealtime(timeWaitingDrain);
        }
    }

    public IEnumerator BreakPlayer(float breakValue)
    {
        //print("clicked");
        IsAnimator.SetBool(MOVING, false);
        IsAnimator.SetBool(STOP, true);
        Hold_timer += Time.deltaTime;
        //print("Stop");
        // StopCoroutine("MovePlayer");
        if (breakValue > 0 && is_hold)
        {

            float elapsedTime = 0;
            while (elapsedTime < max_Hold)
            {
                //Untuk berhenti
                CanoeBody.velocity = Vector2.zero; // Movement
                elapsedTime += Time.deltaTime;
                if (CanoeBody.velocity == Vector2.zero)
                {
                    isMoving = false;
                    MoveBoatSplash.Stop();
                }
                //isMoving = (elapsedTime >= MaxAnimTime) ? true : false;
                breakValue -= Time.deltaTime;
                yield return null;
            }
        }
    }

    //The required method of the IKillable interface
    public void Kill()
    {
        oncharacterDie();
    }

    //The required method of the IDamageable interface
    public void Damage(float damageTaken)
    {

    }

    public void Drain()
    {
        if (Hidup)
        {

            Fatigue();

        }
    }

    public void DistanceTravel()
    {
        float distance = Vector3.Distance(lastMove, transform.position);
        totalDistance += distance;
        lastMove = transform.position;
        DistanceTraveled.text = Mathf.Round(totalDistance) + " m";
        if (!ObjectivesManager.MyInstance.AcomplishObjective(totalDistance, "distance", 0))
        {
            ObjectivesManager.MyInstance.AcomplishObjective(totalDistance, "distance", 0);
        }
    }
    public bool speedLimiter(float Vel)
    {
        float maxMultipliet = maxmultiplier;
        bool LimitReach = false;
        if (Vel >= maxMultipliet)
        {
            LimitReach = true;
        }
        return LimitReach;
    }

    //private void UnityEditorMovement()
    //{
    //    if (Input.GetMouseButtonDown(0) && !isMoving)
    //    {

    //        if (!one_click) // first click no previous clicks
    //        {
    //            one_click = true;
    //            timer_for_double_click = Time.time;
    //            is_hold = true;
    //        }
    //        else
    //        {
    //            //print("Double");
    //            one_click = false; // found a double click, now reset
    //        }

    //    }
    //    if (Input.GetMouseButtonUp(0) || Hold_timer >= max_Hold)
    //    {
    //        is_hold = false;
    //        Hold_timer = 0f;
    //    }
    //    if (is_hold)
    //    {
    //        StartCoroutine(BreakPlayer(MoveSpeedInWater));
    //    }
    //    else
    //    {
    //        is_hold = false;
    //        //jika terlalu lama hold / stop
    //        if (one_click && Hold_timer >= max_Hold)
    //        {

    //            // if the time now is delay seconds more than when the first click started. 
    //            if ((Time.time - timer_for_double_click) > delay)
    //            {
    //                //print("One Hold Time Reached");
    //                StartCoroutine(MovePlayer(0.5f));
    //                one_click = false;
    //            }
    //        }
    //        //jika belum melebihi batas waktu hold
    //        else if (one_click && Hold_timer <= max_Hold - 0.01f)
    //        {

    //            // if the time now is delay seconds more than when the first click started. 
    //            if ((Time.time - timer_for_double_click) > delay)
    //            {
    //                // print("One Hold Time Safe");
    //                StartCoroutine(MovePlayer(MoveSpeed));

    //                MoveBoatSplash.Play();
    //                one_click = false;
    //            }
    //        }
    //        Hold_timer = 0f;
    //    }
    //}
}

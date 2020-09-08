using UnityEngine;

using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : PlayerModel, IChangeable<string>
{
    
    public Player(string name, int point, int movespeed) : base(name, point, movespeed)
    {
    }

    public void Change(string Type)
    {
        Type = CanoeType;
    }

  
    public Status MyExp { get { return ExpStat; } set => ExpStat = value; }

    public int MyScene { get { return scene; } set => scene = value; }

    protected override void Start()
    {


        DontDestroyOnLoad(transform.gameObject);
        if (!playerExists)
        {
            playerExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        base.Start();
    }
    public void SetDefault()
    {

        Energy.Initialize(initEnergy, initEnergy);
        MyExp.Initialize(0, Mathf.Floor(100 * myLevel * Mathf.Pow(myLevel, 0.5f)));
        LevelText.text = myLevel.ToString();

    }

    // Update is called once per frame
    protected override void Update()
    {
        MyScene = SceneManager.GetActiveScene().buildIndex;

        if (!IsAttacking && !IsMoving && Hidup && myEnergy.MyCurrentValue != myEnergy.MyMaxValue)
        {
            EnergyDrain += Time.deltaTime;
            if (EnergyDrain >= batas)
            {
                Regenerate();
                EnergyDrain = 0f;
            }
        }

        base.Update();
    }
    public void Lose(CanvasGroup CGl)
    {
        if (!Hidup)
        {
            CGLose.gameObject.SetActive(true);
            CGl.alpha = CGLose.alpha = 1;
            CGl.blocksRaycasts = CGLose.blocksRaycasts = true;
        }


    }
    public void GainExp(int exp)
    {
        MyExp.MyCurrentValue += exp;

        if (MyExp.MyCurrentValue >= MyExp.MyMaxValue)
        {
            StartCoroutine(Blip());
        }
    }
    private IEnumerator Blip()
    {
        while (!MyExp.ExpFull)
        {
            yield return null;
        }
        myLevel++;
        LevelText.text = myLevel.ToString();
        MyExp.MyMaxValue = 100 * myLevel * Mathf.Pow(myLevel, 0.5f);
        MyExp.MyMaxValue = Mathf.Floor(MyExp.MyMaxValue);
        MyExp.MyCurrentValue = MyExp.MyOverExp;
        MyExp.Reset();
    }
    public void UpdateLevel()
    {
        LevelText.text = myLevel.ToString();
    }
    public void PlayerRespawn()
    {
        Hidup = true;
        IsAnimator.SetBool("Die", false);
        Energy.MyCurrentValue += myEnergy.MyMaxValue;
    }
}
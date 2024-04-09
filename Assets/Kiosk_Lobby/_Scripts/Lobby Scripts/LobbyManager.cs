using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;

    [SerializeField]
    private GameObject Loadingscreen, LoadingScreen2, MainUIScreen,WheelObj, LanguagePanel, UpButtonsPanel,NewsPanel,ShopPanel,ShopSubPanel, ShopSubPanel2,
         RatingPanel,CouponPanel,CouponSubPanel, OfferPanel,FreeEntryPanel,FreeEntryKeyboard1, FreeEntryKeyboard2,CollectPanel,
         CollectPrintPanel,CollectIssuePanel,HelpPanel;

    [SerializeField]
    private GameObject[] OfferRedBttns,offerSubPanels,FreeEntrySubPanels,HelpSubPanels,SidebttnsImages;
    [SerializeField]
    private InputField couponfield ;

    public InputField[] FreeEntryfields;
    int FreeEntryFieldsCount=0;

    [SerializeField]
    private Text coinstxt,freeEntryRedbttnTxt,FreeEntryGreenbttnTxt,AmountTxt,CollectAmountTxt, CollectAmountTxt2, 
        CodeTxt,CodeTxt2,CodeTxt3,RemainingAmountTxt,CollectTime,CollectDate,PrintingStatus,HelpcountTxt,CouponDepositamountTxt,
        CouponBalanceTxt,CouponCodeTxt;

    int AmountInInteger, whichbttn, HelpPanelsCount, CouponDepositamount;
    float totalAmount, totalCoins;
    public Animator sidepanelanim;
    public GameObject sidebttnOutImage;
    public AudioSource ButtonSound,BGsound, keysound,purchasedsound;
    public Button PrintBttn;
    public Image loadingBar;

    public PortSendData senddatascript;
    private void Awake()
    {
        instance = this;
        whichbttn = 1;
        totalCoins = PlayerPrefs.GetFloat("Coins", 0);
        totalAmount = PlayerPrefs.GetFloat("Amount", 0);
    }
    void Start()
    {
        if (Login.isLogin == 1)
        {
            LoadingOff();
        }
        else
        {
            Invoke("LoadingOff", 2.5f);
        }
      //  AmountTxt.text = "$" + totalAmount.ToString("0.00");
        coinstxt.text = "" + totalCoins;
    }
    public void UpdateCoins(string addingamount)
    {
        totalCoins += (float.Parse(addingamount))*100;
        coinstxt.text = "" + totalCoins.ToString("0.00");

        PlayerPrefs.SetFloat("Coins", totalCoins);
        coinstxt.text = "" + totalCoins;
    }
    public void UpdateAmount(string addingamount)
    {
        totalAmount += (float.Parse(addingamount)/100);
       // AmountTxt.text = "$" + totalAmount.ToString("0.00");

        PlayerPrefs.SetFloat("Amount", totalAmount);
    }
    public void LanguagePanelOn()
    {
        ButtonSound.Play();
        UpButtonsPanel.SetActive(false);
        LanguagePanel.SetActive(true);
    }
    public void NewsPanel_ONOff(bool _open)
    {
        ButtonSound.Play();
        NewsPanel.SetActive(_open);
    }
    public void SelectLanguage(int l)
    {
        ButtonSound.Play();
        UpButtonsPanel.SetActive(true);
        LanguagePanel.SetActive(false);
    }
    public void ShopOpenOff(bool _open)
    {
        ButtonSound.Play();
        ShopPanel.SetActive(_open);
    }
    public void ShopItem(bool value)
    {
        ButtonSound.Play();
        ShopSubPanel.SetActive(value);
    }
    public void ShopPurchasedItem(bool value)
    {
        ButtonSound.Play();
        if (value == true)
        {
           // purchasedsound.Play();
        }
        else
        {
         //   ButtonSound.Play();
        }
        ShopSubPanel.SetActive(false);
        ShopSubPanel2.SetActive(value);
    }
    public void RatingOnOff(bool value)
    {
        ButtonSound.Play();
        RatingPanel.SetActive(value);
    }
    #region Coupon
    public void CouponOnOff(bool value)
    {
        couponfield.text = "";
        ButtonSound.Play();
        CouponPanel.SetActive(value);
        CouponSubPanel.SetActive(false);
        ButtonSound.Play();
    }
    public void CouponDeposit()
    {
        if(couponfield.text!="1234")
        {
            return;
        }
        CouponDepositamount = 5;
        totalAmount += CouponDepositamount;
       // CouponDepositamountTxt.text = "$ " + CouponDepositamount;
        CouponBalanceTxt.text = "$ " + totalAmount;
        CouponCodeTxt.text = Random.Range('A', 'Z') + Random.Range(0, 9) +"CA"+ Random.Range(40, 80);
        CouponSubPanel.SetActive(true);
        purchasedsound.Play();

        PlayerPrefs.SetFloat("Amount", totalAmount);
       // AmountTxt.text = "$" + totalAmount.ToString("0.00");

    }
    public void ButtonInput(int number)
    {
        couponfield.text += number;
        keysound.Play();
    }
    public void CouponClear()
    {
        keysound.Play();
        couponfield.text = couponfield.text.Substring(0, couponfield.text.Length - 1);
    }
    #endregion
    public void OfferOnOff(bool value)
    {
        ButtonSound.Play();
        OfferPanel.SetActive(value);
    }
    public void OfferButtons(int b)
    {
        ButtonSound.Play();
       for(int i = 0; i < 3;i++)
        {
            offerSubPanels[i].SetActive(false);
            OfferRedBttns[i].SetActive(false);
        }
        offerSubPanels[b].SetActive(true);
        OfferRedBttns[b].SetActive(true);
    }
    #region Free_Entry
    public void FreeEntryOpen()
    {
        ButtonSound.Play();
        FreeEntryFieldsCount = 0;
        FreeEntryPanel.SetActive(true);
        FreeEntrySubPanels[0].SetActive(true);
        FreeEntryfields[0].text = "";
        freeEntryRedbttnTxt.text = "BACK";
        FreeEntryGreenbttnTxt.text = "NEXT";
        for (int i = 1; i < 5; i++)
        {
            FreeEntrySubPanels[i].SetActive(false);
            FreeEntryfields[i].text = "";
        }
        FreeEntryKeyboard1.SetActive(false);
        FreeEntryKeyboard2.SetActive(true);
    }
    public void FreeEntryFieldsNext(bool isnext)
    {
        ButtonSound.Play();
        FreeEntrySubPanels[FreeEntryFieldsCount].SetActive(false);
        if (FreeEntryFieldsCount == 4 )   //For red button
        {
            FreeEntryPanel.SetActive(false);
            return;
        }
        if (isnext)
        {
            FreeEntryFieldsCount++;
        }
        else
        {
            FreeEntryFieldsCount--;
        }
        if (FreeEntryFieldsCount == 5 || FreeEntryFieldsCount<0)
        {
            FreeEntryPanel.SetActive(false);
            return;
        }
        FreeEntrySubPanels[FreeEntryFieldsCount].SetActive(true);
        if (FreeEntryFieldsCount > 3)
        {
            FreeEntryKeyboard1.SetActive(true);
            FreeEntryKeyboard2.SetActive(false);
            freeEntryRedbttnTxt.text = "EXIT";
            FreeEntryGreenbttnTxt.text = "FINISH";
        }
        else
        {
            FreeEntryKeyboard1.SetActive(false);
            FreeEntryKeyboard2.SetActive(true);
        }
    }
    public void freeEntryKeyNumbers(int number)
    {
        keysound.Play();
        FreeEntryfields[FreeEntryFieldsCount].text += number;
    }
    public void FreeEntryChar(string ch)
    {
        keysound.Play();
        FreeEntryfields[FreeEntryFieldsCount].text += ch;
    }
    public void RemoveTxtFreeEntry()
    {
        keysound.Play();
        FreeEntryfields[FreeEntryFieldsCount].text = FreeEntryfields[FreeEntryFieldsCount].text.Substring(0, FreeEntryfields[FreeEntryFieldsCount].text.Length - 1);

    }
    #endregion

    #region CollectRegion
    public void CollectOpen(bool value)
    {
        if (totalAmount < 1)
        {
            PrintBttn.interactable = false;
        }
        else
        {
            PrintBttn.interactable = true;
        }
        AmountInInteger = (int)totalAmount;
        ButtonSound.Play();
        CollectPanel.SetActive(value);
        CollectAmountTxt.text = "AMOUNT:          $" + AmountInInteger;
        RemainingAmountTxt.text = "REMAINING:    $" + (totalAmount - AmountInInteger).ToString("0.00");
    }
    public void CollectAmount()
    {
        ButtonSound.Play();
        totalAmount -= AmountInInteger;
        //ReadWriteInfo.instance.WriteString(AmountInInteger);
        
        //PortSendData.amounttocollect = AmountInInteger;
        PlayerPrefs.SetFloat("Amount", totalAmount);
        AmountTxt.text = "$" + totalAmount.ToString("0.00");
        Invoke("Sendtoserver", 2f);
        CollectPrint();
    }
    public void CollectPrint()
    {
        ButtonSound.Play();
        CollectPrintPanel.SetActive(true);
        string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string finalstring;
        char[] chars = new char[8];
        for (int i = 0; i < 8; i++)
        {
            chars[i] = Alphabet[Random.Range(0, 36)];
        }
        finalstring = new string(chars);
        PlayerPrefs.SetString("code", finalstring);
        CodeTxt.text = finalstring;
        CodeTxt3.text = finalstring;
        CodeTxt2.text = "CODE:         "+finalstring;
        CollectAmountTxt2.text = "AMOUNT:        $" + AmountInInteger;
        CollectDate.text = "DATE:  " + System.DateTime.Now.ToString("yyyy-MM-dd");
        CollectTime.text = "TIME:  " + System.DateTime.Now.ToString("HH:mm:ss");
        Invoke("CollectCompleted", 2.5f);
    }
    void Sendtoserver()
    {
        PortSendData.instance.SendMessage(AmountInInteger);
    }
    void CollectCompleted()
    {
        purchasedsound.Play();
        PrintingStatus.text = "COMPLETED";
    }
    public void CollectIssue(bool value)
    {
        ButtonSound.Play();
        CollectIssuePanel.SetActive(value);
    }
    public void CollectAllClose()
    {
        ButtonSound.Play();
        CollectPanel.SetActive(false);
        CollectPrintPanel.SetActive(false);
        CollectIssuePanel.SetActive(false);
    }
    #endregion

    public void HelpOnOff(bool value)
    {
        HelpSubPanels[HelpPanelsCount].SetActive(false);
        ButtonSound.Play();
        HelpSubPanels[0].SetActive(true);
        HelpPanel.SetActive(value);
        HelpPanelsCount = 0;
        HelpcountTxt.text = (HelpPanelsCount + 1) + " / 6";
        Sidebttnpress();
    }
    public void Help_Sub_Panels()
    {    
        ButtonSound.Play();
        HelpSubPanels[HelpPanelsCount].SetActive(false);
        HelpPanelsCount++;
        HelpcountTxt.text = (HelpPanelsCount+1) + " / 6";
        if (HelpPanelsCount > 5)
        {
            HelpPanelsCount = 5;
            HelpPanel.SetActive(false);
            Sidebttnpress();
            return;
        }
        HelpSubPanels[HelpPanelsCount].SetActive(true);
    }

    void LoadingOff()
    {
       // BGsound.Play();
        Loadingscreen.SetActive(false);
        WheelObj.SetActive(true);
        MainUIScreen.SetActive(true);
        Login.isLogin = 1;
    }
    public void Sidebttnpress()
    {
        ButtonSound.Play();
        whichbttn++;
        if (whichbttn%2==0)
        {
            sidepanelanim.SetBool("in", true);
            sidebttnOutImage.SetActive(false);
            sidepanelanim.SetBool("out", false);
        }
        else
        {
            sidepanelanim.SetBool("out", true);
            sidepanelanim.SetBool("in", false);
            sidebttnOutImage.SetActive(true);
        }
    }
    public void Sidebuttons(int whichbttn)
    {
        ButtonSound.Play();
        for (int i = 0; i < 4; i++)
        {
            SidebttnsImages[i].SetActive(false);
        }
        SidebttnsImages[whichbttn].SetActive(true);
    }
    public void LoadScenes(int sceneNo)
    {
        MainUIScreen.SetActive(false);
        LoadingScreen2.SetActive(true);
        StartCoroutine(LoadSceneAsync(sceneNo));
    }
    IEnumerator LoadSceneAsync(int SceneNo)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneNo);
        while (!operation.isDone)
        {
            float fillvalue = Mathf.Clamp01(operation.progress / 5f);
            loadingBar.fillAmount += fillvalue;

            yield return null;
        }
    }
    void Update()
    {
        
    }
}

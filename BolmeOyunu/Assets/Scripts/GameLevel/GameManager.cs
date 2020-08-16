using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject karePrefab;
    [SerializeField]
    private Transform karelerPaneli;

    private GameObject[] karelerDizisi = new GameObject[25];

    [SerializeField]
    private Transform soruPaneli;
    [SerializeField]
    private Text soruText;

    [SerializeField]
    private Sprite[] kareSprites;

    List<int> bolumDegerleriListesi = new List<int>();
    int bolunenSayi, bolenSayi;
    int kacinciSoru;
    int butonDegeri;
    bool butonAktifMi=false;
    int dogruSonuc;
    int kalanHak;
    kalanHakManager kalanHaklarManager;
    puanManager puanmanager;
    GameObject gecerliKare;
    string zorluk_Seviyesi;

    [SerializeField]
    private GameObject sonucPanel;
    [SerializeField]
    AudioSource audioSource;
    public AudioClip butonSesi;
    // Start is called before the first frame update
    private void Awake()
    {
        kalanHak = 3;
        audioSource = GetComponent<AudioSource>();
        sonucPanel.GetComponent<RectTransform>().localScale = Vector3.zero;
        kalanHaklarManager = Object.FindObjectOfType<kalanHakManager>();
        kalanHaklarManager.KalanHaklarıKontrolEt(kalanHak);
        puanmanager = Object.FindObjectOfType<puanManager>();
    }
    void Start()
    {
        soruPaneli.GetComponent<RectTransform>().localScale = Vector3.zero;
        kareleriOlustur();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void kareleriOlustur()
    {
        for (int i = 0; i < 25; i++)
        {
            GameObject kare = Instantiate(karePrefab, karelerPaneli);
            kare.transform.GetComponent<Button>().onClick.AddListener(() => ButonaBasildi());
            kare.transform.GetChild(1).GetComponent<Image>().sprite = kareSprites[Random.Range(0, kareSprites.Length)];
            karelerDizisi[i] = kare;
        }
        BolumDegerleriniTexteYazdir();
        StartCoroutine(DoFadeRoutine());
        Invoke("SoruPaneliniAc", 2f);
        
    }
    void ButonaBasildi()
    {
        if (butonAktifMi)
        {
            audioSource.PlayOneShot(butonSesi);
            butonDegeri = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text);
            gecerliKare = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            SonucuKontrolEt();
        }
    }

    void SonucuKontrolEt()
    {
        if (butonDegeri == dogruSonuc)
        {
            gecerliKare.transform.GetChild(1).GetComponent<Image>().enabled = true;
            gecerliKare.transform.GetChild(0).GetComponent<Text>().text = "";
            gecerliKare.transform.GetComponent<Button>().interactable = false;
            
            puanmanager.puaniArtir(zorluk_Seviyesi);
            
            bolumDegerleriListesi.RemoveAt(kacinciSoru);

            if (bolumDegerleriListesi.Count > 0)
            {
                SoruPaneliniAc();
            }
            else
            {
                OyunBitti();
            }

            
        }
        else
        {
            kalanHak--;
            kalanHaklarManager.KalanHaklarıKontrolEt(kalanHak);
            
        }
        if (kalanHak <= 0)
        {
            OyunBitti();
        }
    }
    void OyunBitti()
    {
        butonAktifMi = false;
        sonucPanel.GetComponent<RectTransform>().DOScale(1, 0.3f).SetEase(Ease.OutBack);
    }
    
    IEnumerator DoFadeRoutine()
    {
        foreach (var kare in karelerDizisi)
        {
            kare.GetComponent<CanvasGroup>().DOFade(1, 0.2f);
            yield return new WaitForSeconds(0.05f);
        }
    }

    void BolumDegerleriniTexteYazdir()
    {
        foreach (var kare in karelerDizisi)
        {
            int rastgeleDeger = Random.Range(1, 13);
            bolumDegerleriListesi.Add(rastgeleDeger);
            kare.transform.GetChild(0).GetComponent<Text>().text = rastgeleDeger.ToString();
        }
    }
    

    void SoruPaneliniAc()
    {
        SoruyuSor();
        butonAktifMi = true;
        soruPaneli.GetComponent<RectTransform>().DOScale(1, 0.5f).SetEase(Ease.OutBack);
    }
    void SoruyuSor()
    {
        bolenSayi = Random.Range(2, 11);
        kacinciSoru = Random.Range(0, bolumDegerleriListesi.Count);
        dogruSonuc = bolumDegerleriListesi[kacinciSoru];
        bolunenSayi = bolenSayi * dogruSonuc;
        if (bolunenSayi <= 40)
        {
            zorluk_Seviyesi = "kolay";
        }else if (bolunenSayi>= 40 && bolunenSayi<=80)
        {
            zorluk_Seviyesi = "orta";
        }
        else
        {
            zorluk_Seviyesi = "zor";
        }
        
        soruText.text = bolunenSayi.ToString() + " : " + bolenSayi.ToString();
    }
    
}

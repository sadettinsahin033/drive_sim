using UnityEngine;

public class TrafikArabasi : MonoBehaviour
{
    [Header("Mesafe Ayarlarý")]
    public float hizlanmaMesafesi = 150f;
    public float kaybolmaMesafesi = 500f;

    [Header("Gaz Ayarlarý (0.0 ile 1.0 arasý)")]
    public float uzakMesafeGazi = 0.3f;
    public float yakinMesafeGazi = 1.0f;

    private Transform oyuncuAraci;
    private RCCP_AI yapayZeka;

    // YENÝ: Sistemin yüklenip yüklenmediðini kontrol eden anahtar
    private bool sistemHazirMi = false;

    void OnEnable()
    {
        // Araç her aktif olduðunda önce bekleme moduna geçsin
        sistemHazirMi = false;

        if (yapayZeka == null) yapayZeka = GetComponent<RCCP_AI>();

        if (oyuncuAraci == null)
        {
            GameObject oyuncuObjesi = GameObject.FindGameObjectWithTag("Player");
            if (oyuncuObjesi != null) oyuncuAraci = oyuncuObjesi.transform;
        }

        if (yapayZeka != null) yapayZeka.maxThrottle = uzakMesafeGazi;

        // YENÝ: RCCP'nin kendi iç ayarlarýný yapabilmesi için 2 saniye süre ver
        Invoke("SistemiHazirla", 2f);
    }

    // YENÝ: 2 Saniye sonra bu metot çalýþacak ve aracýn mantýðýný baþlatacak
    void SistemiHazirla()
    {
        sistemHazirMi = true;
    }

    void Update()
    {
        // YENÝ: Eðer 2 saniye dolmadýysa veya gerekli bileþenler eksikse HÝÇBÝR ÞEY YAPMA, BEKLE!
        if (!sistemHazirMi || oyuncuAraci == null || yapayZeka == null) return;

        // --- 2 saniye dolduktan sonra normal kontrollerimiz baþlýyor ---
        float mesafe = Vector3.Distance(oyuncuAraci.position, transform.position);

        if (mesafe > kaybolmaMesafesi)
        {
            gameObject.SetActive(false);
        }
        else if (mesafe <= hizlanmaMesafesi)
        {
            yapayZeka.maxThrottle = yakinMesafeGazi;
        }
        else
        {
            yapayZeka.maxThrottle = uzakMesafeGazi;
        }
    }
}
using UnityEngine;

public class TrafikTetikleyici : MonoBehaviour
{
    [Header("Hedef ve Rota")]
    public Transform spawnNoktasi;
    public RCCP_AIWaypointsContainer gidilecekRota;

    // YENÝ: Sistemin birden fazla kez çalýþmasýný anýnda engelleyecek kilit mekanizmasý
    private bool tetiklendiMi = false;

    private void OnTriggerEnter(Collider other)
    {
        // KÝLÝT KONTROLÜ: Eðer bu tetikleyici daha önce çalýþtýysa hemen iþlemi durdur ve geri dön!
        if (tetiklendiMi) return;

        // Tetikleyiciye giren oyuncu mu?
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            // OYUNCU DEÐDÝÐÝ AN KAPIYI KÝLÝTLE! 
            // Artýk arabanýn arka tekerlekleri veya diðer parçalarý bu kodu ikinci kez çalýþtýramaz.
            tetiklendiMi = true;

            AraciYolaCikar();
        }
    }

    void AraciYolaCikar()
    {
        // 1. Havuzdan müsait bir araç iste
        GameObject yeniArac = TrafikHavuzu.Instance.MüsaitAracVer();

        if (yeniArac != null)
        {
            // 2. Aracý spawn noktasýna yerleþtir
            yeniArac.transform.position = spawnNoktasi.position;
            yeniArac.transform.rotation = spawnNoktasi.rotation;

            // 3. Aracýn rotasýný ver
            RCCP_AI aracAI = TrafikHavuzu.Instance.AIBul(yeniArac);
            if (aracAI != null)
            {
                aracAI.waypointsContainer = gidilecekRota;
                aracAI.currentWaypointIndex = 0; // Rotaya baþtan baþla
            }

            // 4. Aracý aktif et ve yola yolla!
            yeniArac.SetActive(true);

            // 5. Bu tetikleyici iþini yaptý, bir daha oyuncu içinden geçmesin diye kendini kapat
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Havuzda boþ araç kalmadý!");
            // Not: Eðer havuzda araç yoksa, ileride tekrar denenebilmesi için kilidi geri açabiliriz.
            // Fakat senin senaryonda tetikleyici kendini tamamen kapattýðý için buna þu an gerek duymuyoruz.
        }
    }
}
# 🛒 Zara.com Web UI Test Automation

🚀 [**Watch Demo Video (Vimeo)**](https://vimeo.com/1099626336)

---

## 🔧 Project Description / Proje Açıklaması

**EN:**  
This project is a full end-to-end (E2E) test automation suite for the Zara.com shopping experience using C#, Selenium WebDriver, NUnit, EPPlus (Excel), and log4net. It covers product search (from Excel), cart operations, and verification steps as described in the case study scenario.

**TR:**  
Bu proje, Zara.com alışveriş deneyiminin uçtan uca (E2E) otomasyon testini C#, Selenium WebDriver, NUnit, EPPlus (Excel) ve log4net ile gerçekleştirir. Senaryoda belirtilen ürün arama (Excel'den), sepet işlemleri ve doğrulama adımlarını kapsar.

---

## 🚀 Technologies Used / Kullanılan Teknolojiler

- **C# (.NET 9.0)**
- **Selenium WebDriver**
- **ChromeDriver**
- **NUnit**
- **EPPlus** (ExcelReader)
- **log4net**
- **Microsoft.Extensions.Configuration.Json**

---

## 📂 Folder Structure / Klasör Yapısı
WebTests/
├── Config          # appsettings.json, log4net.config
├── Resources       # SearchData.xlsx (Excel keywords)
├── Core            # BaseTest, DriverFactory
├── Pages           # HomePage, LoginPage, SearchPage, ProductPage, CartPage, LogoutPage
├── Utils           # ExcelReader, FileHelper
└── Tests           # ZaraFullE2ETests.cs (main test)

---

## 📃 Configuration / Yapılandırma

- Set **BaseUrl**, login credentials, and output file path in `Config/appsettings.json`.
- Enter search keywords into `Resources/SearchData.xlsx`.  
  Example / Örnek:  
  | şort | gömlek |

> ⚠️ For credentials, a sample file `appsettings.sample.json` is provided.  
> ⚠️ Kimlik bilgileri için `appsettings.sample.json` örneği paylaşılmıştır.

---

## 🚪 How to Run Tests / Testler Nasıl Çalıştırılır?

```sh
cd WebTests
dotnet restore
dotnet test



⸻

📊 Test Flow / Test Akışı

EN:
	1.	Open the site home page
	2.	(Optional) Login (if test account is available & accessible)
	3.	Go to menu: Erkek → “Tümünü Gör”
	4.	Open the search box, enter first Excel keyword (e.g., “şort”), clear it
	5.	Enter the second Excel keyword (“gömlek”), press Enter
	6.	Randomly select a product from search results
	7.	Save product name & price to a .txt file
	8.	Add product to cart
	9.	Verify that the cart price matches the product price
	10.	Change cart quantity to 2, verify
	11.	Remove the product from the cart, check that the cart is empty

TR:
	1.	Site ana sayfasını aç
	2.	(Opsiyonel) Giriş yap (test hesabı ile erişim mümkünse)
	3.	Menüden → Erkek → “Tümünü Gör” seçeneğine tıkla
	4.	Arama kutusunu aç, ilk Excel anahtar kelimesini (“şort”) yaz, temizle
	5.	İkinci Excel anahtar kelimesini (“gömlek”) yaz, Enter’a bas
	6.	Arama sonuçlarından rastgele bir ürün seç
	7.	Ürün adı ve fiyatını .txt dosyasına kaydet
	8.	Ürünü sepete ekle
	9.	Sepet fiyatının ürün fiyatı ile eşleştiğini doğrula
	10.	Sepet adedini 2 yap, kontrol et
	11.	Ürünü sepetten sil, sepetin boş olduğunu kontrol et

⸻

🎨 Sample Output / Örnek Çıktı
[INFO] Home page loaded and cookies accepted.
[INFO] Navigated to Men's section.
[INFO] Opened search input.
[INFO] Searched for keyword: 'şort'
[INFO] Cleared first search input.
[INFO] Searched for keyword: 'gömlek'
[INFO] Random product selected: “Regular Fit Gömlek”
[INFO] Product name and price saved to txt. Product: Regular Fit Gömlek, Price: 1.290,00 TL
[INFO] Product added to cart.
[INFO] Cart price matches product price.
[INFO] Changed cart quantity to 2 and verified.
[INFO] Removed product from cart. Cart is empty.

 Notes / Notlar
	•	EN:
If the site restricts login for automation, the test continues from the product search step. Manual login may be required if site-wide bot protection is active.
	•	TR:
Site, otomasyon için girişe izin vermezse, test ürün arama adımından devam eder. Site genelinde bot koruması varsa manuel giriş gerekebilir.
	•	The Excel file and config files must exist in bin/Debug/net9.0/Resources and Config after build.
	•	Proje aktif olarak geliştirilmektedir; feedback ve katkılarınız memnuniyetle karşılanır.

⸻

👤 Author / Katkı Sağlayan
	•	Hüsnüye — github.com/husnuye

This project is for demo and educational use only. Not affiliated with Zara or Inditex.
Bu proje yalnızca eğitim ve demo amaçlıdır. Zara veya Inditex ile bağlantılı değildir.

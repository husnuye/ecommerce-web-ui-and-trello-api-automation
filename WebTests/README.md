🛒 Zara.com Web Test Automation

🔧 Project Description / Proje Açıklaması

EN:
This project is a full end-to-end (E2E) test automation of Zara.com web shopping experience using C#, Selenium WebDriver, NUnit, EPPlus (Excel), and log4net. It covers searching for products (from Excel data), cart operations, and verification steps as described in the assignment scenario.

TR:
Bu proje, Zara.com alışveriş deneyiminin uçtan uca (E2E) otomasyon testini C#, Selenium WebDriver, NUnit, EPPlus (Excel) ve log4net ile gerçekleştirir. Senaryoda verilen şekilde arama, sepet işlemleri ve doğrulamalar kapsamaktadır.

⸻

🚀 Technologies Used / Kullanılan Teknolojiler
	•	C# (.NET 9.0)
	•	Selenium WebDriver
	•	ChromeDriver
	•	NUnit
	•	EPPlus (ExcelReader)
	•	log4net
	•	Microsoft.Extensions.Configuration.Json

⸻

📂 Folder Structure / Klasör Yapısı

WebTests
 ├── Config          → appsettings.json, log4net.config
 ├── Resources       → SearchData.xlsx (Excel keywords)
 ├── Core            → BaseTest, DriverFactory
 ├── Pages           → HomePage, LoginPage, SearchPage, ProductPage, CartPage, LogoutPage
 ├── Utils           → ExcelReader, FileHelper
 ├── Tests           → ZaraFullE2ETests.cs (main test)


⸻

📃 Configuration / Yapılandırma
	•	Config/appsettings.json içinde sitenin BaseUrl, login bilgileri ve output dosya yolunu belirtin.
	•	Resources/SearchData.xlsx dosyasına arama anahtar kelimelerini girin.
Örnek:
| şort | gömlek |

Not: Gizli bilgiler için appsettings.sample.json ile örnek dosya paylaşılır.

⸻

🚪 How to Run Tests / Testler Nasıl Çalıştırılır?

Terminalden:

cd WebTests
dotnet restore
dotnet test


⸻

📊 Test Flow / Test Akışı

EN:
	1.	Open site home page
	2.	(Optionally) Login (if test account is available & accessible)
	3.	Menu: Erkek → “Tümünü Gör”
	4.	Open search input, enter first Excel keyword (e.g. “şort”), clear it
	5.	Enter second Excel keyword (“gömlek”), press Enter
	6.	Randomly select a product from search results
	7.	Save product name & price to a .txt file
	8.	Add product to cart
	9.	Verify product price matches cart price
	10.	Change cart quantity to 2, verify
	11.	Remove product from cart, check cart is empty


    TR:
    
	1.	Site ana sayfasını aç
	2.	(Opsiyonel) Giriş yap (eğer test hesabı varsa ve erişilebiliyorsa)
	3.	Menüden → Erkek → “Tümünü Gör” seçeneğine tıkla
	4.	Arama kutusunu aç, ilk Excel anahtar kelimesini (örn. “şort”) yaz, sonra temizle
	5.	İkinci Excel anahtar kelimesini (“gömlek”) yaz, Enter’a bas
	6.	Arama sonuçlarından rastgele bir ürün seç
	7.	Ürün adı ve fiyatını bir .txt dosyasına kaydet
	8.	Ürünü sepete ekle
	9.	Sepet fiyatı ile ürün fiyatının eşleştiğini doğrula
	10.	Sepet adedini 2 yap, doğrula
	11.	Ürünü sepetten sil, sepetin boş olduğunu kontrol et


EN:
All steps above are fully automated and outputs (logs, errors, product info) are printed to console and/or written to file.

TR:
Yukarıdaki tüm adımlar otomatik olarak çalıştırılır. Çıktılar ve hata mesajları konsolda ve/veya .txt dosyasında görülebilir.

⸻

📝 Notes / Notlar
	•	Login Step:
If the site restricts login due to test automation, steps continue from product search. Manual intervention may be needed for login if there are site-wide bot protections.
	•	Continuous Development:
The project will be improved during the day. Feedback and extra commits can be shared on Github.
	•	Excel file path and config files must exist in bin/Debug/net9.0/Resources and Config after build.

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


⸻

📌 Contributor / Katkı Sağlayan
	•	Hüsnüye github.com/husnuye
# ✨ Zara.com Web UI Test Automation / Zara.com Web UI Test Otomasyonu

## 🔧 Project Description / Proje Açıklaması

**EN:**  
This project contains automated end-to-end UI tests for Zara.com using Selenium WebDriver with C#. It covers scenarios such as login, search, product selection, adding products to the cart, quantity updates, and removal.

**TR:**  
Bu proje, Selenium WebDriver kullanarak C# ile Zara.com için uçtan uca otomatik web arayüz testi senaryolarını içerir. Senaryolar; giriş yapma, arama, ürün seçimi, sepete ürün ekleme, adet güncelleme ve ürün kaldırmayı kapsar.

---

## 🚀 Technologies Used / Kullanılan Teknolojiler

- C# (.NET 9.0)  
- Selenium WebDriver  
- NUnit  
- ClosedXML (Excel dosyalarını okumak için)  
- log4net (Loglama için)  
- Microsoft.Extensions.Configuration.Json (Yapılandırma yönetimi için)  

---

## 📂 Folder Structure / Klasör Yapısı

EN:

WebTests
├── Config          → Test configuration files
├── Pages           → Page Object Model (page interactions)
├── Resources       → Test data files (e.g. Excel)
├── Tests           → Test scenarios
└── Utils           → Helper classes (ExcelReader, FileHelper, etc.)

TR:
WebTests
├── Config          → Test yapılandırma dosyaları
├── Pages           → Page Object Model (sayfa işlemleri)
├── Resources       → Test veri dosyaları (ör. Excel)
├── Tests           → Test senaryoları
└── Utils           → Yardımcı sınıflar (ExcelReader, FileHelper vb.)

---
📃 Configuration / Yapılandırma

EN:
	•	Test data is read from WebTests/Resources/SearchData.xlsx.
	•	Necessary settings are configured in files under WebTests/Config.
	•	Logging is managed via log4net.config.

TR:
	•	Test verileri WebTests/Resources/SearchData.xlsx dosyasından okunur.
	•	Gerekli ayarlar WebTests/Config klasöründeki dosyalarda yapılır.
	•	Loglama log4net.config ile kontrol edilir.
---

## 🚪 How to Run Tests / Testler Nasıl Çalıştırılır?

cd WebTests
dotnet test

EN:
	1.	Navigate to the WebTests project folder.
	2.	Run the tests using the command above.
	3.	Test results will be displayed in the console and saved to log files.

TR:
	1.	WebTests proje klasörüne gidin.
	2.	Yukarıdaki komutu kullanarak testleri çalıştırın.
	3.	Test sonuçları konsola yazdırılacak ve log dosyalarına kaydedilecektir.




📊 Test Flow / Test Akışı

Step 1 / Adım 1:
EN: Open the homepage and accept cookies if present.
TR: Ana sayfa açılır ve varsa cookie banner kapatılır.

Step 2 / Adım 2:
EN: Click the login button and submit valid user credentials.
TR: Giriş butonuna tıklanır, kullanıcı bilgileri girilerek giriş yapılır.

Step 3 / Adım 3:
EN: Navigate to the Men’s section and click the “See All” link.
TR: Erkek kategorisine gidilir ve “TÜMÜNÜ GÖR” linkine tıklanır.

Step 4 / Adım 4:
EN: Open the search box and enter the first keyword from Excel.
TR: Arama kutusu açılır, Excel’den ilk anahtar kelime girilir.

Step 5 / Adım 5:
EN: Clear the search input and enter the second keyword.
TR: Arama kutusu temizlenir, Excel’den ikinci anahtar kelime girilir.

Step 6 / Adım 6:
EN: Submit the search by pressing the Enter key.
TR: Enter tuşuna basılarak arama gönderilir.

Step 7 / Adım 7:
EN: Select the first product from the search results.
TR: Arama sonuçlarından ilk ürün seçilir.

Step 8 / Adım 8:
EN: Retrieve product name and price; save details to a text file.
TR: Ürün adı ve fiyatı alınır; detaylar dosyaya kaydedilir.

Step 9 / Adım 9:
EN: Add the product to the cart, select size, and handle any popups.
TR: Ürün sepete eklenir, beden seçilir ve varsa popup yönetilir.

Step 10 / Adım 10:
EN: Verify the product price in the cart matches the product page price.
TR: Sepetteki ürün fiyatı, ürün sayfasındaki fiyat ile karşılaştırılır.

Step 11 / Adım 11:
EN: Change product quantity to 2 in the cart and verify the update.
TR: Sepette ürün adedi 2 olarak değiştirilir ve doğrulanır.

Step 12 / Adım 12:
EN: Remove the product from the cart and verify the cart is empty.
TR: Ürün sepetten çıkarılır ve sepetin boş olduğu doğrulanır.







📖 Notes / Notlar
	•	Dynamic elements are handled using explicit waits (WebDriverWait).
	•	Popups and modals have dedicated handling logic.
	•	Excel reading is done via ClosedXML.
	•	Logging is detailed using log4net.
	•	Page Object Model (POM) design pattern is used.

⸻

🙌 Contributions / Katkılar

Pull requests are welcome. Feel free to contribute with new tests, bug fixes, or improvements.
Pull request’ler açıktır. Yeni testler, hata düzeltmeleri veya iyileştirmeler ekleyebilirsiniz.

⸻

🌍 License / Lisans

MIT License

⸻

📚 References / Kaynaklar
	•	Selenium WebDriver Documentation
	•	ClosedXML GitHub
	•	NUnit Documentation
	•	log4net Documentation

⸻

For detailed test scenarios, configuration, and running instructions, please refer to the folders and README files inside this project.
Detaylı test senaryoları, yapılandırma ve çalıştırma için proje klasörlerini ve README dosyalarını inceleyiniz.
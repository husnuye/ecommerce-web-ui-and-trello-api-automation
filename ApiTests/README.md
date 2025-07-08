# ✨ Trello API Test Automation

## 🔧 Project Description / Proje Açıklaması

**EN:**  
This project demonstrates automated API testing of Trello using C#, NUnit, and RestSharp. It covers full CRUD operations (Create, Read, Update, Delete) for Trello Boards and Cards.

**TR:**  
Bu proje, Trello API üzerinde C#, NUnit ve RestSharp kullanarak otomatik testler içerir. Board ve Card işlemleri için tam CRUD (oluştur, oku, güncelle, sil) süreçlerini kapsar.

---

## 🚀 Technologies Used / Kullanılan Teknolojiler

- **C# (.NET 9.0)**
- **NUnit**
- **RestSharp**
- **Newtonsoft.Json**
- **Microsoft.Extensions.Configuration.Json**

---

## 📂 Folder Structure / Klasör Yapısı

ApiTests/
├── Config      # ConfigHelper & api.settings.json
├── Core        # ApiClient (request handler)
├── Models      # BoardModel.cs, CardModel.cs
├── Utils       # JsonHelper.cs (future), ConfigHelper.cs
└── Tests       # TrelloBoardTests.cs (main test file)

---

## 📃 Configuration / Yapılandırma

Create `Config/api.settings.json` as below:

```json
{
  "BaseUrl": "https://api.trello.com/1/",
  "ApiKey": "YOUR_TRELLO_API_KEY",
  "Token": "YOUR_TRELLO_TOKEN"
}

---

⚠️ Do NOT share your real credentials. Commit only api.settings.sample.json to the repository!

---

🚪 How to Run Tests / Testler Nasıl Çalıştırılır?

Terminal:
cd ApiTests
cp Config/api.settings.json bin/Debug/net9.0/Config/
dotnet test

---

📊 Test Flow / Test Akışı
	1.	Create Trello Board
	2.	Get list from the board
	3.	Create two cards
	4.	Randomly update a card
	5.	Delete cards
	6.	Delete board

---

EN:
All steps above are performed automatically. Logs and results are printed to the terminal.

TR:
Yukarıdaki adımlar otomatik olarak yürütülür. Log ve sonuçlar terminalde gösterilir.

Board created: abc123
List retrieved: xyz456
Cards created: card1, card2
Card updated: card1
Cards deleted.
Board deleted.

---

🔐 API Configuration
	•	NEVER commit your real api.settings.json file.
	•	Use api.settings.sample.json as a template for others.

To run:
EN: Copy the sample file and fill your credentials.
TR: Örnek dosyayı kopyalayıp kendi Trello bilgilerinizi girin.

cp ApiTests/Config/api.settings.sample.json ApiTests/Config/api.settings.json

🛡️ Notes / Notlar
	•	Do NOT share your API key or token with anyone.
	•	Add api.settings.json to your .gitignore.
	•	All logs use TestContext.WriteLine() for traceability.

---

🙌 Contributions / Katkılar

EN:
Pull requests are welcome. Feel free to contribute additional features, models, or test cases.

TR:
Pull request’ler açıktır. Ek özellik, model veya test senaryosu ekleyebilirsiniz.

---

🌍 License / Lisans

MIT
---

Author / Yazar: github.com/husnuye

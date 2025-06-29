# ✨ Trello API Test Automation

## 🔧 Project Description / Proje Açıklaması

**EN:**
This project demonstrates automated API testing of Trello using C#, NUnit, and RestSharp. It performs full CRUD operations (Create, Read, Update, Delete) for Trello Boards and Cards.

**TR:**
Bu proje, Trello API'leri üzerinde C#, NUnit ve RestSharp kullanarak otomatik testlerin gerçekleştirildiği bir uygulamadır. Board ve Card işlemleri için tam CRUD (oluşur, oku, güncelle, sil) süreçleri test edilmektedir.

---

## 🚀 Technologies Used / Kullanılan Teknolojiler

* C# (.NET 9.0)
* NUnit
* RestSharp
* Newtonsoft.Json
* Microsoft.Extensions.Configuration.Json

---

## 📂 Folder Structure / Klasör Yapısı

```
ApiTests
 ├── Config          → ConfigHelper & api.settings.json
 ├── Core            → ApiClient (request handler)
 ├── Models          → BoardModel.cs, CardModel.cs
 ├── Utils           → JsonHelper.cs (future), ConfigHelper.cs
 ├── Tests           → TrelloBoardTests.cs (main test file)
```

---

## 📃 Configuration / Yapılandırma

> `Config/api.settings.json` dosyasını oluşturun ve aşağıdaki gibi doldurun:

```json
{
  "BaseUrl": "https://api.trello.com/1/",
  "ApiKey": "YOUR_TRELLO_API_KEY",
  "Token": "YOUR_TRELLO_TOKEN"
}
```

---

## 🚪 How to Run Tests / Testler Nasıl Çalıştırılır?

**Terminalden:**

```bash
cd ApiTests
cp Config/api.settings.json bin/Debug/net9.0/Config/
dotnet test
```

---

## 📊 Test Flow / Test Akışı

1. Create Trello Board
2. Get List from Board
3. Create Two Cards
4. Randomly Update a Card
5. Delete Cards
6. Delete Board

**EN:**
All tests automatically perform these steps when executed and log the outputs to the terminal.

**TR:**
Tüm testler, çalıştığında otomatik olarak bu adımları gerçekleştirir ve logları terminale yazar.

---

## 🎨 Sample Output / Örnek Çıktı

```bash
Board created: abc123
List retrieved: xyz456
Cards created: card1, card2
Card updated: card1
Cards deleted.
Board deleted.
```

---

## 🛡️ Notes / Notlar

* API key ve token bilgilerinizi **kimseyle paylaşmayın**.
* `api.settings.json` dosyasını `.gitignore` listesine ekleyin.
* Geliştirme yaparken test çıktıları `TestContext.WriteLine()` ile loglanmıştır.

---

## 🙌 Contributions / Katkılar

**EN:**
Pull requests are welcome. Feel free to contribute with additional features, models, or test scenarios.

**EN:**
Pull request'ler açıktır. Ek işlemler, model ya da test senaryoları eklemekten çekinmeyin.

---

**EN:**
📌 To run the tests, copy `api.settings.sample.json` → `api.settings.json` and fill in your own Trello credentials.


**EN:**
📌 Testleri çalıştırmak için `api.settings.sample.json` dosyasını `api.settings.json` olarak kopyalayın ve kendi Trello bilgilerinizi girin.

## 🌍 License / Lisans

MIT

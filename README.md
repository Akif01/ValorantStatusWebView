# 🛡️ Valorant Status Web View

A Blazor Server web application that shows real-time **Valorant platform server status** for all major regions using Riot Games' public API. The app is optimized with asynchronous refresh, manual cooldowns, and Docker-based deployment.

---

## ✨ Features

- 🔄 Auto-refresh every 30 seconds
- 📍 Shows current **availability** per region (e.g., EU, NA, AP, KR)
- 🚨 Error handling and retry logic
- 🔐 Riot API key injected securely via Docker secrets
- ⚙️ NGINX reverse proxy
- 🐳 Docker-ready

---

## 🧪 Running the Project with 🐳 Docker

### Prerequisites

- [Docker](https://docs.docker.com/get-docker/)

### Steps

1. Clone the repo:

   ```bash
   git clone https://github.com/Akif01/ValorantStatusWebView.git
   cd ValorantStatusWebView

2. Add your API key to a file named valorant_api_key.txt:

   ```bash
   cd ValorantStatusWebView
   echo YOUR_RIOT_API_KEY > valorant_api_key.txt

3. Run all services
   ```bash
   docker-compose up --build

4. Open your browser
   
   http://localhost

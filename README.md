# PlayerBack - API Backend

Une API .NET 8 (ASP.NET Core) pour la gestion des joueurs, construite avec une architecture en couches (Domain, Application, Infrastructure, API).

## 📋 Table des matières

- [Prérequis](#prérequis)
- [Installation](#installation)
- [Lancer l'application](#lancer-lapplication)
- [Tester l'application](#tester-lapplication)
- [Structure du projet](#structure-du-projet)
- [Configuration](#configuration)
- [Endpoints](#endpoints)
- [Terminologie](#terminologie)

## 🧭 Terminologie

- **.NET 8**: runtime et SDK utilisés pour compiler et exécuter l'application.
- **ASP.NET Core**: framework web construit au-dessus de .NET pour créer des APIs/Applications web.
- Dans ce projet, on a une application **ASP.NET Core** qui cible **.NET 8** (voir `Sdk="Microsoft.NET.Sdk.Web"` et `TargetFramework=net8.0`).

## 🔧 Prérequis

Avant de commencer, assurez-vous d'avoir installé:

- **[.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)** ou version supérieure
- **[MongoDB](https://www.mongodb.com/try/download/community)** (ou un accès à une base de données MongoDB cloud)
- **Git** (pour cloner le repository)
- **Un éditeur de code** comme [Visual Studio Code](https://code.visualstudio.com/) ou [Visual Studio 2022](https://visualstudio.microsoft.com/fr/)

### Vérifier les installations

```bash
# Vérifier la version de .NET
dotnet --version

# Vérifier MongoDB
mongosh --version
```

## 📦 Installation

### 1. Cloner le repository

```bash
git clone <repository-url>
cd PlayerBackend.NET
```

### 2. Restaurer les dépendances

```bash
dotnet restore
```

## ▶️ Lancer l'application

### Méthode 1: Ligne de commande

```bash
# Se placer à la racine du projet
cd src/PlayerBack.Api

# Lancer l'application en mode développement
dotnet run

# Ou avec la configuration de lancement recommandée
dotnet run --launch-profile https
```

L'application sera disponible à:
- **URL HTTPS**: `https://localhost:7001`
- **URL HTTP**: `http://localhost:5230`
- **Swagger UI**: `https://localhost:7001/swagger/index.html`

### Méthode 2: Visual Studio

1. Ouvrir `PlayerBack.sln` dans Visual Studio
2. Mettre `PlayerBack.Api` comme projet de démarrage (clic droit → "Set as Startup Project")
3. Appuyer sur **F5** ou cliquer sur "▶ Démarrer"

### Méthode 3: Visual Studio Code

1. Ouvrir le dossier du projet dans VS Code
2. Appuyer sur **F5** pour démarrer le débogage
3. Sélectionner `.NET` comme environnement si demandé

## 🧪 Tester l'application

### Vérifier la santé de l'application

Une fois l'application lancée, vérifiez qu'elle fonctionne:

```bash
# Endpoint de santé
curl https://localhost:7001/health
```

### Interface Swagger UI

La documentation interactive de l'API est accessible via Swagger:

1. Ouvrir `https://localhost:7001/swagger/index.html` dans votre navigateur
2. Voir tous les endpoints disponibles
3. Tester directement les endpoints depuis l'interface

### Lancer les tests unitaires

#### Tous les tests

```bash
dotnet test
```

#### Tests d'un projet spécifique

```bash
# Tests de l'API
dotnet test test/PlayerBack.Api.UnitTests/PlayerBack.Api.UnitTests.csproj

# Tests de l'Application
dotnet test test/PlayerBack.Application.UnitTests/PlayerBack.Application.UnitTests.csproj

# Tests du Domain
dotnet test test/PlayerBack.Domain.UnitTests/PlayerBack.Domain.UnitTests.csproj

# Tests de l'Infrastructure
dotnet test test/PlayerBack.Infrastructure.UnitTests/PlayerBack.Infrastructure.UnitTests.csproj
```

#### Avec rapport de couverture

```bash
dotnet test --collect:"XPlat Code Coverage"
```

#### Lancer les tests avec diagnostic verbose

```bash
dotnet test --verbosity=detailed
```

### Tests manuels avec curl ou Postman

#### Exemple: Récupérer les données ensemencées

```bash
curl -X GET "https://localhost:7001/api/players" \
  -H "accept: application/json"
```

#### Exemple: Créer une nouveau joueur (POST)

```bash
curl -X POST "https://localhost:7001/api/players" \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "John",
    "lastName": "Doe",
    "email": "john@example.com"
  }'
```

## 🏗️ Structure du projet

```
PlayerBackend.NET/
├── src/                              # Code source
│   ├── PlayerBack.Api/              # Couche API (Controllers, Middleware)
│   │   ├── Controllers/             # Contrôleurs API
│   │   ├── Middleware/              # Middlewares personnalisés
│   │   ├── Program.cs               # Configuration et démarrage
│   │   └── appsettings.*.json       # Fichiers de configuration
│   ├── PlayerBack.Application/      # Logique métier (Services)
│   ├── PlayerBack.Domain/           # Entités et règles métier
│   │   ├── Models/                  # Modèles de domaine
│   │   ├── Dtos/                    # Data Transfer Objects
│   │   └── Mapping/                 # Mappage d'objets
│   └── PlayerBack.Infrastructure/   # Accès aux données (BD)
│       ├── Seeding/                 # Initialisation des données
│       └── Common/                  # Classes utilitaires
├── test/                            # Tests unitaires
│   ├── PlayerBack.Api.UnitTests/
│   ├── PlayerBack.Application.UnitTests/
│   ├── PlayerBack.Domain.UnitTests/
│   └── PlayerBack.Infrastructure.UnitTests/
├── PlayerBack.sln                   # Solution Visual Studio
└── README.md                        # Ce fichier
```

## ⚙️ Configuration

### Configuration de la base de données MongoDB

Les paramètres de connexion sont définis dans les fichiers `appsettings.json`:

**appsettings.Development.json** (développement):
```json
{
  "MongoDB": {
    "ConnectionString": "mongodb+srv://playerback:playerback@playerback.7hetqvd.mongodb.net/?appName=PlayerBack",
    "DatabaseName": "Dev"
  }
}
```

**appsettings.json** (production):
```json
{
  "AllowedHosts": "*"
}
```

### Modification de la configuration

Pour développement local avec MongoDB local, modifiez `appsettings.Development.json`:

```json
{
  "MongoDB": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "Dev"
  }
}
```

## 🔌 Endpoints

### Santé

- `GET /health` - Vérifie l'état de santé de l'API

### Joueurs (À personnaliser selon votre API)

- `GET /api/Player/Players` - Récupère la liste des joueurs
- `GET /api/Player/Player/{id}` - Récupère un joueur par ID
- `GET /api/Player/GetStatistics` - Récupère Les pays qui a le plus grand ratio de parties gagnées, IMC moyen de tous les joueurs, la médiane de la taille des joueurs
- `POST /api/Player/CreatePlayer` - Crée un nouveau joueur

> Consultez la page Swagger pour la liste complète des endpoints et leurs paramètres: `https://localhost:7001/swagger`

## 🐛 Débogage

### Visual Studio Code

1. Installer l'extension **C#** (ms-dotnettools.csharp)
2. Appuyer sur **F5** pour commencer le débogage
3. Ajouter des points d'arrêt en cliquant sur la marge gauche

### Visual Studio

1. Ajouter des points d'arrêt en cliquant sur la marge gauche
2. Appuyer sur **F5** pour lancer le débogage
3. Utiliser la fenêtre "Debug" pour inspecter les variables

## 🚀 Déploiement

### Build pour la production

```bash
dotnet build -c Release
```

### Publier l'application

```bash
dotnet publish -c Release -o ./publish
```

## 📚 Ressources utiles

- [Documentation .NET 8.0](https://learn.microsoft.com/fr-fr/dotnet/)
- [Documentation ASP.NET Core](https://learn.microsoft.com/fr-fr/aspnet/core/)
- [MongoDB Documentation](https://docs.mongodb.com/)
- [Swagger/OpenAPI](https://swagger.io/)

## 📝 Notes supplémentaires

- L'application utilise **Entity Framework Core** ou un driver MongoDB pour accéder à la base de données
- L'architecture suit le pattern **Clean Architecture**
- Les données de test sont ensemencées automatiquement au démarrage en mode développement
- La gestion globale des exceptions est implémentée via un middleware personnalisé
- Les health checks sont disponibles pour la surveillance

## 🤝 Support

Pour toute question ou problème, consultez:
1. La documentation de l'API (Swagger)
2. Les tests unitaires comme exemples d'utilisation
3. Les fichiers de configuration dans `appsettings.json`

---

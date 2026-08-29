# BiblioManager

**BiblioManager** est une application de gestion de bibliothèque développée avec **ASP.NET Core Web API**.

L'application permet de gérer les utilisateurs, les adhérents, les livres et les emprunts, avec un système d'authentification, d'autorisation et de paiement.

> 🚧 Projet en cours de développement

## Fonctionnalités

### 🔐 Authentification & sécurité

* Inscription et connexion
* Authentification avec **JWT**
* Gestion des rôles :

  * Admin
  * Employe
  * Utilisateur
  * Adherent
* Autorisation des fonctionnalités selon le rôle
* Protection des données personnelles
* Gestion centralisée des erreurs

### 👤 Utilisateurs

* Création d'un compte
* Consultation et modification du profil
* Gestion des rôles par l'administrateur
* Suppression d'un utilisateur selon ses permissions

### 👥 Adhérents

* Devenir adhérent
* Activation de l'adhésion après paiement
* Gestion de la durée de l'adhésion
* Gestion des pénalités
* Renouvellement de l'adhésion
* Désactivation automatique d'une adhésion expirée

### 📚 Livres

* Ajouter un livre
* Consulter les livres
* Consulter un livre
* Modifier un livre
* Supprimer un livre
* Gestion des quantités disponibles

### ✍️ Auteurs

* Ajouter un auteur
* Consulter les auteurs
* Consulter un auteur
* Modifier un auteur
* Supprimer un auteur

### 🏷️ Catégories

* Ajouter une catégorie
* Consulter les catégories
* Modifier une catégorie
* Supprimer une catégorie
* Protection de la catégorie par défaut

### 📖 Emprunts

* Emprunter un livre
* Retourner un livre
* Consulter l'historique des emprunts
* Détection des emprunts en retard
* Gestion des statuts d'emprunt
* Mise à jour automatique des quantités disponibles

### 💳 Paiements

* Initiation d'un paiement par carte
* Gestion des paiements d'abonnement
* Gestion des paiements de pénalités
* Suivi du statut des paiements
* Activation / renouvellement de l'adhésion après paiement

## Technologies

* **C#**
* **ASP.NET Core Web API**
* **Entity Framework Core**
* **SQL Server**
* **JWT**
* **BCrypt**
* **Swagger / OpenAPI**
* **Git / GitHub**

## Architecture

Le backend est organisé en plusieurs couches :

```text
Controllers
     ↓
Services
     ↓
Repositories
     ↓
Entity Framework Core
     ↓
SQL Server
```

Les **DTOs**, **Mappers** et **Middleware** sont utilisés pour structurer et sécuriser l'API.

## API

L'API est documentée et testable avec **Swagger**.

Les endpoints protégés utilisent l'authentification JWT et les autorisations basées sur les rôles.

## Fonctionnalités à venir

* [ ] Refresh Token
* [ ] Intégration Stripe Webhook
* [ ] Tests unitaires et d'intégration
* [ ] Docker
* [ ] CI/CD
* [ ] Frontend Angular
* [ ] Application d'administration WinForms

## Statut

🚧 **En développement**

De nouvelles fonctionnalités et améliorations sont ajoutées progressivement.

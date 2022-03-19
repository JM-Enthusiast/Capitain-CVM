using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Représente les données de jeu
/// </summary>
[Serializable]
public class PlayerData
{
    /// <summary>
    /// Niveau sélectionné par l'utilisateur pour le vol. général
    /// </summary>
    [Range(-80, 0)]
    private float _volumeGeneral;
    public float VolumeGeneral { get { return _volumeGeneral; } set { _volumeGeneral = value; } }

    /// <summary>
    /// Niveau sélectionné par l'utilisateur pour le vol. de la musique
    /// </summary>
    [Range(-80, 0)]
    private float _volumeMusique;
    public float VolumeMusique { get { return _volumeMusique; } set { _volumeMusique = value; } }

    /// <summary>
    /// Niveau sélectionné par l'utilisateur pour le vol. de la musique
    /// </summary>
    [Range(-80, 0)]
    private float _volumeEffet;
    public float VolumeEffet { get { return _volumeEffet; } set { _volumeEffet = value; } }

    /// <summary>
    /// Représente le nombre de points de vie du personnage
    /// </summary>
    private int _vie;
    /// <summary>
    /// Représente le nombre d'énergie (entre 0 et 4)
    /// </summary>
    private int _energie;
    /// <summary>
    /// Représente le score obtenu
    /// </summary>
    private int _score;
    /// <summary>
    /// Liste des coffres ouverts dans le jeu
    /// </summary>
    private List<string> _chestOpenList;
    /// <summary>
    /// Représente le maximum d'énergie du personnage
    /// </summary>
    public const int MAX_ENERGIE = 4;
    /// <summary>
    /// Permet d'identifier les actions sur le UI à réaliser
    /// lors de la perte d'énergie
    /// </summary>
    public Action UIPerteEnergie;
    /// <summary>
    /// Permet d'identifier les actions sur le UI à réaliser
    /// lors de la perte d'énergie
    /// </summary>
    public Action UIPerteVie;
    /// <summary>
    /// Permet d'identifier les actions à réaliser lors d'un gameover
    /// </summary>
    public Action Gameover;

    public int Energie { get { return _energie; } }
    public int Vie { get { return _vie; } }
    public int Score { get { return _score; } }
    public string[] ListeCoffreOuvert { get { return _chestOpenList.ToArray(); } }

    // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/auto-implemented-properties
    // Ce syntactic sugar va automatiquement créer les champs privés associés aux propriétés
    // L'équivalent de ce qui a été fait pour la vie, le score, etc. mais en une seule ligne
    public HashSet<string> UnlockedHelmets { get; }
    public HashSet<string> CompletedLevels { get; }

    public PlayerData()
    {
        _vie = 0;
        _energie = 0;
        _score = 0;
        _volumeGeneral = 0;
        _volumeMusique = 0;
        _volumeEffet = 0;
        UIPerteEnergie = null;
        UIPerteVie = null;
        Gameover = null;
        _chestOpenList = new List<string>();
        UnlockedHelmets = new HashSet<string>();
        CompletedLevels = new HashSet<string>();
    }

    public PlayerData(int vie = 1, int energie = 2, int score = 0,
        float volumeGeneral = 0, float volumeMusique = 0, float volumeEffet = 0,
        Action uiPerteEnergie = null, Action uiPerteVie = null,
        Action gameOver = null, List<string> ChestList = null, List<string> unlockedHelmets = null, List<string> completedLevels = null)
    {
        _vie = vie;
        _energie = energie;
        _score = score;
        _volumeGeneral = volumeGeneral;
        _volumeMusique = volumeMusique;
        _volumeEffet = volumeEffet;
        UIPerteEnergie += uiPerteEnergie;
        UIPerteVie += uiPerteVie;
        Gameover += gameOver;
        _chestOpenList = new List<string>();
        if (ChestList != null)
            _chestOpenList = ChestList;
        UnlockedHelmets = new HashSet<string>();
        if (unlockedHelmets != null)
            UnlockedHelmets = new HashSet<string>(unlockedHelmets);
        CompletedLevels = new HashSet<string>();
        if (completedLevels != null)
            CompletedLevels = new HashSet<string>(completedLevels);
    }

    /// <summary>
    /// Diminue l'énergie du personnage
    /// </summary>
    /// <param name="perte">Niveau de perte (par défaut 1)</param>
    public void DecrEnergie(int perte = 1)
    {
        _energie -= perte;
        UIPerteEnergie();
        if (_energie <= 0)
        {
            DecrVie();
        }
    }

    /// <summary>
    /// Permet de réduire la vie d'un personnage
    /// </summary>
    public void DecrVie()
    {
        _vie--;
        UIPerteVie();
        if (_vie <= 0)
            Gameover();
        else
        {
            IncrEnergie(MAX_ENERGIE);
            GameManager.Instance.RechargerNiveau();
        }
    }

    /// <summary>
    /// Permet d'augmenter l'énergie jusqu'à MAX_ENERGIE
    /// </summary>
    /// <param name="gain">Gain d'augmentation</param>
    public void IncrEnergie(int gain)
    {
        _energie += gain;
        if (_energie > MAX_ENERGIE)
        {
            _energie = 1;
            IncrVie();
        }
        
        UIPerteEnergie();
    }

    /// <summary>
    /// Permet d'augmenter la vie
    /// </summary>
    /// <param name="gain">Gain d'augmentation</param>
    public void IncrVie(int gain = 1)
    {
        _vie += gain;
        UIPerteVie();
    }

    /// <summary>
    /// Augmente le score du joueur
    /// </summary>
    /// <param name="gain">Point gagné</param>
    public void IncrScore(int gain = 1)
    {
        _score += gain;
    }

    /// <summary>
    /// Ajoute le nom du coffre à la liste
    /// </summary>
    /// <param name="nom">Nom du coffre à ajouter</param>
    public void AjouterCoffreOuvert(string nom)
    {
        _chestOpenList.Add(nom);
    }

    /// <summary>
    /// Détermine si le coffre est contenu dans la liste
    /// des coffres ouverts
    /// </summary>
    /// <param name="nom">Nom du coffre à vérifier</param>
    /// <returns>true si le coffre est ouvert, false sinon</returns>
    public bool AvoirOuvertureCoffre(string nom)
    {
        return _chestOpenList.Contains(nom);
    }

    /// <summary>
    /// Adds the level to the set
    /// </summary>
    /// <param name="level">Name of the completed level</param>
    public void CompleteLevel(string level)
    {
        CompletedLevels.Add(level);
    }
    
    /// <summary>
    /// Check whether the specified level has been completed
    /// </summary>
    /// <param name="level">Name of the level to check</param>
    /// <returns>true if it has been completed, false otherwise</returns>
    public bool HasCompletedLevel(string level)
    {
        return CompletedLevels.Contains(level);
    }

    /// <summary>
    /// Adds the helmet to the set
    /// </summary>
    /// <param name="level">Name of the level containing the helmet</param>
    public void AddUnlockableHelmet(string level)
    {
        UnlockedHelmets.Add(level);
    }

    /// <summary>
    /// Check whether the specified helmet has been unlocked
    /// </summary>
    /// <param name="level">Name of the level containing the helmet to check</param>
    /// <returns>true if it has been unlocked, false otherwise</returns>
    public bool HasUnlockedHelmet(string level)
    {
        return UnlockedHelmets.Contains(level);
    }
}

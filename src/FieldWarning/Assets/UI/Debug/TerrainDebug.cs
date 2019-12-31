﻿using PFW;
using PFW.Model.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainDebug : MonoBehaviour
{
    private MatchSession _matchSession = null;

    // Start is called before the first frame update
    void Start()
    {
        var gameSession = GameObject.FindWithTag("GameSession");
        _matchSession = gameSession.GetComponent<MatchSession>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Util.GetTerrainClickLocation(out hit);

        var positionField = transform.Find("PositionField").GetComponent<Text>();
        positionField.text = hit.point.ToString();

        var terrainType = _matchSession.TerrainMap.GetTerrainType(hit.point);

        var typeField = transform.Find("TypeField").GetComponent<Text>();
        typeField.text = terrainType.ToString();
    }

    public void ReloadTerrainData()
    {
        _matchSession.TerrainMap.ReloadTerrainData();
    }
}

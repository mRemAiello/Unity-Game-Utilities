using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace GameUtils
{
    public class GridPlacementAdvanced : MonoBehaviour
    {
        // Parametri per la griglia
        public int rows = 2;    // Numero di righe
        public int columns = 2; // Numero di colonne

        // Range per la spaziatura casuale tra gli oggetti
        public float xMin = 1f; // Minima distanza sull'asse X
        public float xMax = 3f; // Massima distanza sull'asse X
        public float yMin = 1f; // Minima distanza sull'asse Y
        public float yMax = 3f; // Massima distanza sull'asse Y

        [Button]
        private void PositionChildrenInRandomGrid()
        {
            int currentRow = 0;
            int currentColumn = 0;
            float xOffset = 0f;
            float yOffset = 0f;

            // Posiziona ogni figlio nella griglia
            foreach (Transform child in transform)
            {
                // Calcola una spaziatura casuale tra gli oggetti
                float randomXSpacing = Random.Range(xMin, xMax);
                float randomYSpacing = Random.Range(yMin, yMax);

                // Calcola la nuova posizione basata sulla spaziatura casuale
                Vector3 newPosition = new Vector3(
                    currentColumn * randomXSpacing + xOffset,
                    child.position.y,
                    currentRow * randomYSpacing + yOffset
                );

                // Imposta la nuova posizione
                child.position = newPosition;

                // Incrementa la colonna e aggiorna la riga quando necessario
                currentColumn++;
                if (currentColumn >= columns)
                {
                    currentColumn = 0;
                    currentRow++;
                }
            }
        }

        // Disegna Gizmos per rappresentare la griglia
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            float xOffset = 0f;
            float yOffset = 0f;

            // Disegna i punti della griglia nella scena
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    // Calcola una spaziatura casuale per i Gizmos
                    float randomXSpacing = Random.Range(xMin, xMax);
                    float randomYSpacing = Random.Range(yMin, yMax);

                    Vector3 gridPosition = new Vector3(
                        col * randomXSpacing + xOffset,
                        transform.position.y,
                        row * randomYSpacing + yOffset
                    );

                    Gizmos.DrawWireCube(gridPosition, new Vector3(1, 1, 1)); // Disegna un cubo wireframe per ogni punto
                }
            }
        }
    }
}
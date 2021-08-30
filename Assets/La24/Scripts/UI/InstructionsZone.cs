using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsZone : MonoBehaviour
{


      public GameObject Content;
      private RectTransform ContentPosition;
      public Text Paginas;
      private int PaginaActual;
      private int NumeroDePaginas;
      private int SaltoDePagina;

      private void Start()
      {
            ContentPosition = Content.GetComponent<RectTransform>();
            SaltoDePagina = 920;
            PaginaActual = 1;
            NumeroDePaginas = ((4734 - 134) / SaltoDePagina) + 1;
            
            Paginas.text = "Página: " + PaginaActual + "/" + NumeroDePaginas;
      }

      public void Subir()
      {
            Vector2 position = ContentPosition.anchoredPosition;
            if (position.y <= 134)
            {
                  return;
            }
            ContentPosition.anchoredPosition = new Vector2(position.x, position.y - SaltoDePagina);
            PaginaActual--;
            Paginas.text = "Página: " + PaginaActual + "/" + NumeroDePaginas;
      }

      public void Bajar()
      {
            Vector2 position = ContentPosition.anchoredPosition;
            if (position.y >= 4734)
            {
                  return;
            }
            ContentPosition.anchoredPosition = new Vector2(position.x, position.y + SaltoDePagina);
            PaginaActual++;
            Paginas.text = "Página: " + PaginaActual + "/" + NumeroDePaginas;  
      }
}

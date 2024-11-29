using System.Collections;
using TMPro;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] int q;                       //Valores de p y q
    [SerializeField] int p;
    [SerializeField] TMP_Text qPressingsUI;       //Cuenta las veces que se pulsan p o q
    [SerializeField] TMP_Text pPressingsUI;
    [SerializeField] int bigSize;                 //Tamaño de Fuente
    [SerializeField] int mediumSize;
    [SerializeField] int smallSize;
    [SerializeField] Color winColor;              //Colores
    [SerializeField] Color evenColor;
    [SerializeField] Color loseColor;
    [SerializeField] GameObject qVictoryUI;       //Carteles de Victoria
    [SerializeField] GameObject pVictoryUI;
    [SerializeField] GameObject cartelReinicioUI; //Cartel de reinicio
    [SerializeField] int qVictorias;              //Numero de victorias
    [SerializeField] int pVictorias;
    [SerializeField] TMP_Text qTextoVictorias;    //Contador de victorias
    [SerializeField] TMP_Text pTextoVictorias;
    private bool partidaTerminada = false;
    private int inicioContador = 5;
    private bool contador = true;
    [SerializeField] TMP_Text textoContadorUI;
    [SerializeField] GameObject cuentaAtrasContadorUI;
    private float inicioTransicion = 0f;
    private bool canPressG = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Contador());
        StartCoroutine(EnableGKeyPress());
    }

    // Update is called once per frame
    void Update()
    {
        if (canPressG && Input.GetKeyDown(KeyCode.G)) // Usar G para empezar nuevo juego
        {
            canPressG = false;
            StartCoroutine(EnableGKeyPress());
            StartNewGame();
            contador = true;
            StartCoroutine(Contador());
        }
        if (q == 10 || p == 10)  //Fin de partida
        {
            if (q == 10) //Cuando gana Q
            {
                print("Has ganado Q!");
                qVictorias++;
                Victoria();
                PrintVictorias();
            }
            else //Cuando gana P
            {
                print("Has ganado P!");
                pVictorias++;
                Victoria();
                PrintVictorias();
            }
            p = 0;
            q = 0;
            partidaTerminada = true;
            StartCoroutine(FinalizarPartida());
        }
        else if (!partidaTerminada && !contador) //Juego en ejecucion
        {
            if (Input.GetKeyDown(KeyCode.Q)) // Sumar uno a Q
            {
                q++;
                PrintValues();
            }
            if (Input.GetKeyDown(KeyCode.P)) //Sumar uno a P
            {
                p++;
                PrintValues();
            }
            if (q > p) //Que pasa si q es mayor que p
            {
                qPressingsUI.fontSize = bigSize;
                pPressingsUI.fontSize = smallSize;
                qPressingsUI.color = winColor;
                pPressingsUI.color = loseColor;
            }
            else if (p > q) //que pasa si p es mayor que q
            {
                pPressingsUI.fontSize = bigSize;
                qPressingsUI.fontSize = smallSize;
                pPressingsUI.color = winColor;
                qPressingsUI.color = loseColor;
            }
            else //que pasa si p y q son iguales
            {
                pPressingsUI.fontSize = mediumSize;
                qPressingsUI.fontSize = mediumSize;
                qPressingsUI.color = evenColor;
                pPressingsUI.color = evenColor;
            }
        }
    }

    void PrintValues() //Enseña cuantas pulsaciones llevamos.
    {
        print("q: " + q + " p: " + p);

        qPressingsUI.text = q.ToString();
        pPressingsUI.text = p.ToString();
    }

    void PrintVictorias()  //Contador de victorias.
    {
        print("Num Victorias Q: " + qVictorias + " Num Victorias P: " + pVictorias);

        qTextoVictorias.text = qVictorias.ToString();
        pTextoVictorias.text = pVictorias.ToString();
    }

    void Victoria() 
    {
        if (q == 10)  //Esto se ejecuta cuando llega q o p a 10 para ver el mensaje final.
            qVictoryUI.SetActive(true);
        else if (p == 10)
            pVictoryUI.SetActive(true);
        cartelReinicioUI.SetActive(true);
        canPressG = true;
    }

    IEnumerator FinalizarPartida() // Esconde los carteles de victoria y reinicio.
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.G));
        StopAllCoroutines();
        qVictoryUI.SetActive(false);
        pVictoryUI.SetActive(false);
        cartelReinicioUI.SetActive(false);
        StartNewGame();
    }

    void StartNewGame() // Resetea variables y reinicia la partida
    {
        p = 0;
        q = 0;
        pPressingsUI.fontSize = mediumSize;
        qPressingsUI.fontSize = mediumSize;
        qPressingsUI.color = evenColor;
        pPressingsUI.color = evenColor;
        qPressingsUI.text = q.ToString();
        pPressingsUI.text = p.ToString();
        partidaTerminada = false;
        contador = true;
        inicioContador = 5;
        StartCoroutine(Contador());
    }

    IEnumerator Contador() //Cuenta atrás de empezar el juego. IEnumerator es una funcion pero devuelve una variable.
    {
        cuentaAtrasContadorUI.SetActive(true);
        qTextoVictorias.gameObject.SetActive(true);
        pTextoVictorias.gameObject.SetActive(true);
        textoContadorUI.text = inicioContador.ToString();
        while (inicioContador > 0) 
        {
            yield return new WaitForSeconds(1);  //yield asigna un valor en memoria para que return se ejecute de nuevo
            inicioContador--; 
            textoContadorUI.text = inicioContador.ToString();
        }
        contador = false;
        cuentaAtrasContadorUI.SetActive(false);
        yield return new WaitForSeconds(1);
        qTextoVictorias.gameObject.SetActive(false);
        pTextoVictorias.gameObject.SetActive(false);
    }

    IEnumerator TransicionG()
    {
        cartelReinicioUI.SetActive(false);
        while (inicioTransicion < 1f)
        { 
            yield return null;
            inicioTransicion = (inicioTransicion + 0.0001f);
        }
        cartelReinicioUI.SetActive(true);
    }

    IEnumerator EnableGKeyPress()
    {
        yield return new WaitForSeconds(5);
        canPressG = true; 
    }
}

# Relazione progetto di Programmazione e Modellazione ad Oggetti (PMO)
## Specifica
Il progetto consiste nella realizzazione di un _parser_ per la specifica di rappresentazione dati [BTC](https://github.com/GeckoNickDeveloper/BluetoothTagChain). Suddetto parser dovrà consentire sia la codifica in testo (o scrittura su file) che la decodifica da testo (o file) in oggetto per garantire una gestione lato codice. Per la codifica sarà necessario creare un oggetto appropriato da serializzare.
## Studio del problema
Dalla [specifica](https://github.com/GeckoNickDeveloper/BluetoothTagChain) insorgono delle problematiche che necessitano di essere affrontate:
- Rappresentazione della struttura dati
- Decoding del testo
- Encoding dell'oggetto in testo
Delle possibili soluzioni possono essere l'utilizzo del **_Composite Pattern_** per la rappresentazione della struttura dati, mentre per l'algoritmo di decoding bisogna analizzare più a fondo le fasi principali. Al momento, esse potrebbero essere riassunte come:
- _Acquisizione stringa_
- _Semplificazione stringa_
- _Parsing oggetto_ (da ripetere i passi sottostanti fino alla chiusura dell'oggetto o fino al verificarsi di un errore di sintassi)
	- _Parsing elemento_
		- _Parsing TAG_
		- _Parsing VALUE_
- _Restituzione Oggetto letto_<br>
L'algoritmo di encoding può essere visto a grandi linee come il precedente, con la differenza che qui i dati sono già esistenti, quindi sarà "molto" più semplice trascriverne il contenuto in testo.

## Scelte architetturali
Nella sezione precedente, abbiamo visto delle possibili soluzioni per la realizzazione del progetto. Andremo ora a vedere come sono state utilizzate in pratica per risolvere le problematiche sopracitate.
#### Struttura dati
Al fine di rappresentare la struttura dati ad _albero_ descritta nella specifica [BTC](https://github.com/GeckoNickDeveloper/BluetoothTagChain), si è optato per il **_Composite Pattern_**: esso è un pattern strutturale solitamente utilizzato per la creazione di alberi; consiste in una interfaccia implementata da tutte le classi facenti parte dell'albero (**Component**) e _X_ classi rappresentanti i nodi e le foglie.<br>
In questo caso, necessitiamo di tre diversi tipi di foglie: numeri, stringhe e valori booleani. I nodi complessi, invece, sono due: oggetti e liste, dove l'implementazione è abbastanza diversa da meritare due classi separate.<br>
In questa implementazione le varie classi prendono i seguenti nomi:
- BTCNumber
- BTCString
- BTCBool<br>
Per la rappresentazione delle _foglie_, mentre:
- BTCObject
- BTCList<br>
Rappresentano i _nodi_ dell'albero, ovvero gli oggetti "complessi".<br><br>
Ciascuna delle classi sopracitate implementa un'interfaccia comune:
- IBTCData<br>
La quale espone due metodi per la codifica in testo (uno con ed uno senza parametri).<br><br>
Tale scelta ha portato ad un grosso vantaggio per quanto riguarda la codifica in testo ed ha, allo stesso tempo, gettato delle basi di partenza per l'algoritmo di parsing.

#### Algoritmi
Come accennato nello studio del problema e, successivamente, nella progettazione della struttura dati, l'algoritmo di _encoding_ consisterà in uno scorrimento degli elementi dei nodi complessi, invocandone a sua volta l'algoritmo di encoding, fino ad arrivare a tutte le foglie, completando così la codifica.<br><br>
Dall'altra parte della medaglia, tuttavia, l'algoritmo di decoding risulta più complicato, in quanto deve NON SOLO convertire il testo in dati, ma anche effettuare una verifica della sintassi. Per ovviare a questa soluzione, è stata creata una classe rappresentante un errore nella sintassi del testo (BTCSyntaxErrorException); quest'ultima viene sollevata quando insorgono dei problemi di sintassi e può essere gestita da un utente che utilizza la libreria.<br>
Tornando all'algoritmo di decoding, una valida soluzione può essere quella che utilizza una metodologia _greedy_, dove ogni qual volta che ci imbattiamo in un nodo ne avviamo subito il parsing, lasciando in sospeso il nodo padre, per poi tornarci non appena tutti i parsing invocati (sempre con lo stesso metodo) saranno conclusi. Per ridurre al minimo la complessità (in termini di codice, non performance) si è optato di di suddividere l'algoritmo in tanti sotto-algoritmi, con conseguente beneficio di maggior semplicità di debugging.<br><br>
Tutti gli algoritmi sopracitati sono raggruppati all'interno della classe statica _BTCParser_.

#### Diagramma delle classi
Il seguente _diagramma delle classi_ è il risultato dell'unione della progettazione della struttura dati e degli algoritmi utilizzati:<br>
![Diagramma UML delle classi](https://github.com/GeckoNickDeveloper/Progetto-PMO/blob/master/Relazione/src/BTC.jpg)

## Documentazione sull'utilizzo
Siccome il progetto consiste essenzialmente in una libreria, verranno descritte in seguito tutte le funzionalità esposte.
#### Funzionalità BTCParser
Le funzionalità principali esposte dalla libreria consistono in dei metodi
- _Encode_: questo metodo permette di convertire un oggetto di tipo BTCObject in stringa, permettendo di specicare anche se la stringa debba essere formattata in maniera compatta o "bella" (default);
- _EncodeIntoFile_: come il precedente, con la differenza che andrà a scrivere la string all'interno di file (il percorso ed il nome del file devono essere specificati);
- _Decode_: permette di creare un'istanza di BTCObject partendo da una stringa (questo metodo solleva un'eccezione di tipo BTCSyntaxErrorException in caso di errore di sintassi)
- _DecodeFromFile_: come il precedente, però richiede il percorso del file anziché il testo da decodificare;
- _Normalize_: metodo per trasformare una stringa in formato compatto.
#### Classi utilizzabili
La libreria mette a disposizione 5 classi, le quali consentono di creare istanze più o meno complesse che possono essere convertite in testo dai metodi della classe _BTCParser_.
#### Linking
Il progetto è stato sviluppato su Manjaro (Arch Linux), tramite l'utilizzo del tool `dotnet`; di conseguenza è necessario specificare i passaggi per utilizzare la libreria BTC all'interno di altri progetti scritti in _C#_, per lo meno in ambiente Linux:<br><br>
Entrare nella directory del progetto dove si vuole utilizzare la libreria ed eseguire il seguente comando:
```
dotnet add reference "../BTC/"
```
dove la stringa `../BTC/` è il percordo alla directory del progetto BTC (locazione dove è stata salvata la directory del progetto).

## Use Cases con relativo schema UML

# Relazione progetto di Programmazione e Modellazione ad Oggetti (PMO)
## Specifica
Il progetto consiste nella realizzazione di un PARSER per la specifica [BTC](https://github.com/GeckoNickDeveloper/BluetoothTagChain). Suddetto PARSER dovrà consentire sia la codifica in testo (e/o scrittura su file) che la decodifica da testo (o file). Per la codifica sarà necessario creare un oggetto da serializzare.
## Studio del problema
Dalla [specifica](https://github.com/GeckoNickDeveloper/BluetoothTagChain) del file di testo, notiamo subito che vi sono tre principali problematiche per la realizzazione del progetto:
1. La rappresentazione della struttura ad albero
2. Algoritmo di parsing (decoding)
3. Algoritmo di encoding


## Scelte architetturali
In questa sezione andremo a spiegare le principali scelte architetturali. Esse hanno l'intento di essere il più indipendenti possibili dal linguaggio, anche se alcune particolarità sono state espresse con le parole chiave del linguaggio C# (in cui questo progetto software è stato scritto).

#### Struttura dati
Durante la fase di modellazione del progetto, è capitato più volte di creare diagrammi UML. Inizialmente, per la rappresentazione della struttura dati di tipo _albero_, vi sono stati numerosi possibili diagrammi delle classi dovuti all'aggiornamento delle specifiche BTC, che tuttavia non risultavano efficaci e portavano solo inutile complessità all'interno del progetto vero e proprio. Si è dunque optato per l'utilizzo di un design pattern particolarmente adatto a rappresentare _alberi_: il **_Composite Pattern_**. Ma anche allora esistevano diverse soluzioni che si basavano sul _pattern_ scelto.<br>
Una consisteva nel creare 3 **classi chiave** ed 1 interfaccia comune (_Component_ nella definizione del pattern), dove ogni classe chiave implementava l'interfaccia comune:
- `IBTCData` (interfaccia)
- `BTCObject`
- `BTCList`<T>
- `BTCElement`<T>
Tuttavia questa soluzione implicava che venissero create delle **sottoclassi di specializzazione** per _BTCElement_ e _BTCList_; quindi venne scartata in quanto avrebbe causato una eccessiva complessità del _package_ ed avrebbe dato troppa libertà all'utente che avrebbe utilizzato questa libreria. Libertà che avrebbe potuto portare il progetto a rivelarsi inutile. Inoltre, con uno degli aggiornamenti della specifica BTC, si è rivelato un modello non consono alla rappresentazione.<br>
Un'altra possibile implementazione consisteva in:
- `IBTCData` (interfaccia)
- `BTCObject`
- `BTCList : List<IBTCData>`
- `BTCElement<T>`<br>
Ancora una volta, però, l'ipotesi è stata abbandonata, in quanto esponeva troppe funzionalità rispetto all'utilizzo previsto, nonchè alla necessità di creare classi di specializzazione che estendessero `BTCElement<T>` lasciando all'utente la possibilità di "fare danni".<br>
L'ultima soluzione che vedremo all'interno di questo documento, sarà quella effettivamente adottata. Tale soluzione consiste in 5 classi (più solita interfaccia) dove, nonostante l'apparente inefficienza, ogni classe è specializzata e non permette all'utente finale di "rompere" la libreria in se. In questo caso, quindi:
- `BTCString`
- `BTCNumber`
- `BTCBool`
- `BTCObject`
- `BTCList`
Dove le classi semplici sono le prime tre, mentre quelle complesse (le ultime due) espongono metodi per la sola gestione delle classi elencate (più qualche metodo per le informazioni importanti).

#### Algoritmi
Dal punto di vista algoritmico, invece, vi è stata più attenzione. Innanzituto è stata creata la classe _BTCParser_ con lo scopo di esporre dei metodi che permettano la codifica e decodifica di un file BTC. Siccome questi metodi non necessitano della creazione di un'istanza, sono definiti come statici (come tutti gli altri metodi della classe).

Le funzionalità esposte fanno uso dei seguenti algoritmi:
- Manipolazione della stringa dati:
	- _Normalize_
- Utilità di conversione:
	- _TryParse_
- Parsing:
	- _ParseObject_
	- _ParseList_
	- _ParseString_
	- _ParseElement_
	- _ParseItem_

Iniziamo descrivendo l'unico algoritmo di **manipolazione della stringa dati**: _Normalize_.<br>
Questo algoritmo serve per rimuovere da una stringa il padding (andate a capo, spazi, tabulazioni). In questo modo la stringa risultante (non viene sovrascritta l'originale) sarà più compatta (o al massimo uguale) rispetto all'originale. In questo modo permettiamo una migliore gestione della stringa, sia ne volessimo fare il parsing, sia l'utente voglia trasmetterla tramite Bluetooth. Questo metodo è, infatti, reso pubblico per offrire una migliore gestione della stringa dati.<br><br>

Parliamo ora delle **utilità di conversione**: _TryParse_.<br>
Questo algoritmo ci permette di convertire stringhe in valori numerici e booleani (se possibile), notificandoci del successo o dell'insuccesso dell'operazione.
Bisogna però dire che è l'unico algoritmo che ha necessità di cambiare, anche radicalmente, a seconda del linguaggio utilizzato.<br><br>

Entriamo ora nel cuore della libreria: gll algoritmi di **parsing**.<br>
Questo particolare algoritmo utilizza una metodologia _greedy_: non appena trova un carattere che segnala possibile elemento o item, prova ad eseguirne subito il parsing, lasciando in sospeso l'operazione precedente fino a completamento del nuovo parsing iniziato. Questa tecnica ci torna particolarmente comoda in quanto ogni elemento nella specifica BTC è una coppia TAG-VALORE, dove il valore può essere un numero, una stringa, un valore booleano o addirittura un oggetto o una lista, i quali a loro volta contengono ELEMENTI o ITEM, quindi un strategia di parsing di questo tipo ci permette di arrivare fino ad un valore semplice per poi tornare indietro e continuare con il parsing degli elementi più complessi.<br>
Tuttavia, potrebbe capitare che si verifichino degli errori di sintassi. Per ovviare a questo problema è stata creata la classe _BTCSyntaxErrorException_: una classe che estende Exception

#### Diagramma delle classi
Come risultato della modellazione e della progettazione della struttura dati e degli algoritmi utilizzati, è nato questo _diagramma delle classi_:<br>
![Diagramma UML delle classi](https://github.com/GeckoNickDeveloper/Progetto-PMO/blob/master/Relazione/src/BTC.jpg)

## Documentazione sull'utilizzo
[Questo](https://github.com/GeckoNickDeveloper/Progetto-PMO/tree/master/BTC) software non richiede compilazione per essere eseguito, in quanto consiste in una libreria (con relativo DLL) importabile in qualsiasi progetto software svilutppato tramite linguaggio _C#_.<br>
Per testarla, invece, è già stato realizzato un [programma](https://github.com/GeckoNickDeveloper/Progetto-PMO/tree/master/Testing) di testing dove andiamo a generare un file `generated.btc` con delle informazioni fittizie, leggerlo nuovamente effettuandone il PARSING ed aggiungendogli un TAG con relativo valore per, infine, riscriverlo nello stesso file sopracitato.

> NOTA:<br>
> Tutto il progetto è stato scritto, compilato ed eseguito su piattaforma Linux (nello specifico sulla
> distribuzione Manjaro, derivata di Arch) tramite tool `dotnet` e `Visual Studio Code`.

Una documentazione sull'utilizzo delle funzionalità pubbliche esposte dalla libreria (classi, interfacce) è disponibile all'interno della repository in '/docs/html/index.html' ed è facilmente consultabile da browser una volta clonata la repository in locale. 

## Use Cases con relativo schema UML
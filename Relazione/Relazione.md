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

#### struttura dati
Durante la fase di modellazione del progetto, è capitato più volte di creare diagrammi UML. Inizialmente, per la rappresentazione della struttura dati di tipo _albero_, vi sono stati numerosi possibili diagrammi delle classi dovuti all'aggiornamento delle specifiche BTC, che tuttavia non risultavano efficaci e portavano solo inutile complessità all'interno del progetto vero e proprio. Si è dunque optato per l'utilizzo di un design pattern particolarmente adatto a rappresentare _alberi_: il **_Composite Pattern_**. Ma anche allora esistevano diverse soluzioni che si basavano sul _pattern_ scelto.<br>
Una consisteva nel creare 3 **classi chiave** ed 1 interfaccia comune (_Component_ nella definizione del pattern), dove ogni classe chiave implementava l'interfaccia comune:
- `IBTCData` (interfaccia)
1. `BTCObject`
2. `BTCList`<T>
3. `BTCElement`<T>
Tuttavia questa soluzione implicava che venissero create delle **sottoclassi di specializzazione** per _BTCElement_ e _BTCList_; quindi venne scartata in quanto avrebbe causato una eccessiva complessità del _package_ ed avrebbe dato troppa libertà all'utente che avrebbe utilizzato questa libreria. Libertà che avrebbe potuto portare il progetto a rivelarsi inutile. Inoltre, con uno degli aggiornamenti della specifica BTC, si è rivelato un modello non consono alla rappresentazione.<br>
Un'altra possibile implementazione consisteva in:
- `IBTCData` (interfaccia)
1. `BTCObject`
2. `BTCList : List<IBTCData>`
3. `BTCElement<T>`
Ancora una volta, però, l'ipotesi è stata abbandonata, in quanto esponeva troppe funzionalità rispetto all'utilizzo previsto, nonchè alla necessità di creare classi di specializzazione che estendessero `BTCElement<T>` lasciando all'utente la possibilità di "fare danni".<br>
L'ultima soluzione che vedremo all'interno di questo documento, sarà quella effettivamente adottata. Tale soluzione consiste in 5 classi (più solita interfaccia) dove, nonostante l'apparente inefficienza, ogni classe è specializzata e non permette all'utente finale di "rompere" la libreria in se. In questo caso, quindi:
1. `BTCString`
2. `BTCNumber`
3. `BTCBool`
4. `BTCObject`
5. `BTCList`
Dove le classi semplici sono quelle numerate da 1 a 3, mentre quelle complesse (4 e 5) espongono metodi per la sola gestione delle classi elencate (più qualche metodo per le informazioni importanti).

#### algoritmi
Dal punto di vista algoritmico, invece, vi è stata più attenzione. Innanzituto è stata creata la classe _BTCParser_ con lo scopo di esporre dei metodi che permettano la codifica e decodifica di un file BTC. Siccome questi metodi non necessitano della creazione di un'istanza, sono definiti come statici (come tutti gli altri metodi della classe)






#### Diagramma delle classi
Come risultato della modellazione e della progettazione della struttura dati e degli algoritmi implementati, è nato questo _diagramma delle classi_:<br>
![Diagramma UML delle classi](https://github.com/GeckoNickDeveloper/Progetto-PMO/blob/master/Relazione/src/BTC.jpg)

### Documentazione sull'utilizzo
[Questo](https://github.com/GeckoNickDeveloper/Progetto-PMO/tree/master/BTC) software non richiede compilazione per essere eseguito, in quanto consiste in una libreria (con relativo DLL) importabile in qualsiasi progetto software svilutppato tramite linguaggio _C#_.<br>
Per testarla, invece, è già stato realizzato un [programma](https://github.com/GeckoNickDeveloper/Progetto-PMO/tree/master/Testing) di testing dove andiamo a generare un file `generated.btc` con delle informazioni fittizie, leggerlo nuovamente effettuandone il PARSING ed aggiungendogli un TAG con relativo valore per, infine, riscriverlo nello stesso file sopracitato.

> NOTA:<br>
> Tutto il progetto è stato scritto, compilato ed eseguito su piattaforma Linux (nello specifico sulla
> distribuzione Manjaro, derivata di Arch) tramite tool `dotnet` e `Visual Studio Code`.

Una documentazione sull'utilizzo delle funzionalità pubbliche esposte dalla libreria (classi, interfacce) è disponibile all'interno della repository in '/docs/html/index.html' ed è facilmente consultabile da browser una volta clonata la repository in locale. 

### Use Cases con relativo schema UML
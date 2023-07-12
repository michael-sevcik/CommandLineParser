# Prvni dojem

`*` README obsahuje minimalni uvod, prehled hlavnich konceptu, jednoduchy i
slozitejsi priklad pouziti, build instrukce pro knihovnu a vygenerovani
referencni dokumentace.

`~` Layout repozitare je neusporadany.

  * Sady testu jsou ve trech adresarich s naprosto odlisnymi jmeny, bylo
    by vhodne je umistit do spolecneho podadresare.

  * Podobne projekty s priklady by bylo vhodne nejak seskupit.

`*` Knihovna jde prelozit, testy prochazi, existuje CI setup.


# Dokumentace

`*` Projekt obsahuje konfiguraci pro Doxygen, jako titulni stranka generovane
dokumentace je pouzit obsah README, vybrane tridy odkazuji na externi soubory
integrovane do referencni dokumentace.

`~` Referencni dokumentace nejde vygenerovat bez upravy `Doxyfile` (nutno
nahradit absolutni cesty relativnimi).

`*` Popis hlavnich trid pochazi z externi dokumentace. U nekterych tridy k ni
nemam prilis vyhrad, u jinych obsahuje spise jen seznam metod pro dany typ.
Celkove by bylo vhodne, aby to byla dokumentace generovana ze zdrojovek.

Dokumentace ve zdrojovkach je nejblize kodu a tedy ma potencial byt nejlepe
udrzovana, na rozdil od externi dokumentace, ktera muze zastarat.

Proto by externi dokumentace (napr. pouzita pro titulni stranku generovane
dokumentace, nebo jina "rucne" vytvarena dokumentace) mela poskytovat spise
hlavni obrysy, ale detailnejsi popis trid a jejich role a vzajemna interakce v
kontextu knihovny by mela pochazet z generovane dokumentace.

`~` Popis hlavnich trid v externi dokumentaci nema hyperlinky na zminovane
tridy a metody. Ty by mely vest do generovane dokumentace.

`-` Popis trid ve zdrojovkach je minimalisticky, coz se odrazi v generovane
dokumentaci. Vetsina tridy ma pouze brief popis, ktery nic detailnejsiho
nerika. Vice informaci je v externi dokumentaci.

`~` Knihovna vypada, ze podporuje rozsirovani, ale neni jasne jak. Dokumentace
se zminuje o rozhranich `IOption`, `IParametrizedOption` a
`IMultipleParameterOption`, ale chybi priklad pro typicke situace, kdy by
chtel uzivatel knihovnu rozsirovat (parsovani custom typu ci custom validace).


# Rozhrani knihovny

`*` Imperativni rozhrani s fluent konfiguraci a funkcionalnimi prvky. Za
ukladani hodnot je zodpovedny uzivatel, ktery musi parseru dodat callback,
ktery s hodnotou nalozi. Knihovnu je mozne rozsirit o vlastni implementaci
pojmenovanych a pozicnich parametru.

`!` S pozicnimi a pojmenovanymi parametry se pracuje ruznymi zpusoby.

Pozicni parametry je potreba predat konstruktoru, pojmenovane parametry se
pridavaji postupne. Oboji by se melo definovat podobne, hlavni rozdil je v
podstate pouze ve zpusobu identifikace argumentu na prikazove radce. Jestlize
mate `OptionBuilder`, tak by mel byt existovat podobny mechanizmus pro pozicni
argumenty.

`!` API micha dva ruzne typy stavu.

Trida `Parser` udrzuje nejen informace o konfiguraci parseru (coz by se dalo
celkem pochopit), ale take informace, ktere souvisi s poslednim volanim metody
`ParseCommandLine()` (informace o chybach a zbyvajicich pozicnich
argumentech). Metoda `ParseCommandLine()` by spise mela vracet objekt, ktery
by reprezentoval vysledek parsovani.

`!` Rozhrani neni dostatecne robustni vuci chybam v konfiguraci.

Metoda `ParseCommandLine()` vraci typ `bool`, ktery indikuje chybu, na kterou
se uzivatel musi doptat pomoci property. Neni ale jasne, jak jsou podchyceny
pripady, kdy je konfigurace parseru spatne.

  * Ani v prikladech se nijak neresi navratova hodnota metody
    `RegisterOption()`. Chyby v konfiguraci by uzivately nemely "projit",
    proto by bylo vhodne vyhazovat vyjimky. Pak uz by bylo vhodne chybove
    chovani sjednotit a vyhazovat vyjimky i pri parsovani uzivatelskeho
    vstupu.

  * Specifikace omezeni je spolecna pro vsechny typy options bez ohledu na
    typ. Neni jasne, jak se bude metoda chovat pri pouziti na spatnem typu.

`-` Verejne API obsahuje radu nepotrebnych metod.

  * Metoda `Reset()` na builderech je diskutabilni, protoze neni jasne, proc
    by bylo potreba recyklovat buildery. To by totiz zpusobilo, ze definice
    jednotlivych options je zavisla na poradi v kodu. Vhodnejsi je mit pro
    kazdou option vlastni builder a trochu k tomu uzivatele tlacit.

  * Neni jasne, proc maji buildery metodu `RegisterOption()`, uzivatel
    by si mohl instanci option do parseru pridat sam, kdyz uz to trida
    `Parser` umoznuje.

  * Metoda `CreateParticularOptionForRegistration()` by se mela jmenovat spise
  `Build()`

  * Metoda `parseInt()` nema na builderu co delat.

  * Rozhrani obsahuje radu ruznych variant `With*Action()`. Minimalne
    vicenasobne hodnoty by pravdepodobne slo reprezentovat pomoci typu.

`-` Rozsirovani knihovny je prilis komplikovane a rozhrani pro to urcena
nejsou dostatecne oddelena od bezneho pouziti.

  * Rozhrani `IOption`, `IParametrizedOption`, `IMultipleParameterOption`, a
    `IPlainArgument` poskytuji metody, ktere muze uzivatel bezne volat i
    na objektech vytvorenych knihovnou (napr. pokud misto `RegisterOption()`
    zavola `CreateParticularOptionForRegistration()`).

  * Rozhrani jsou prilis slozita pro bezna rozsireni, kde je potreba
    definovat vlastni konverzi retezcove reprezentace hodnoty na hodnotovy
    typ, pripadne definovat vlastni validaci hodnot.

  * Neni jasne, proc by tato rozhrani vubec mela obsahovat metodu `Restore()`.

  * Metoda `TakeAction()` vyzaduje, aby implementace rozhrani udrzovala
    vnitrni stav (typicky tedy vysledek predchoziho volani
    `ProcessParameter()`) a na pozadani s nim neco provedla. To znamena, ze
    custom implementace techto rozhrani musi z velke casti duplikovat chovani
    options implementovanych knihovnou, misto toho, aby dodala jen kousky
    chovani, ktere stoji za to customizovat.


# Zduvodneni navrhu

`!` Postradam nejaky design dokument, ktery by popsal hlavni koncepty a hlavni
use cases &mdash; veci, co se typicky rikaly pri prezentaci. Navic by se zde
hodilo rozebrat duvody pro radu designovych rozhodnuti v API.


# Kvalita implementace

`-` Trida `OptionBuilder` obsahuje radu statickych metod pro parsovani
hodnot.

Tyto metody uz nesouvisi s konfiguraci a by bylo vhodne je mit v tridach
reprezentujicich specificke implementace `IOption`, nebo v nejake jine tride.

`-` Implementace `IOption` a dalsich rozhrani jsou v podstate pouze datove
tridy, veskere chovani je nekde jinde.


`!` Nektere metody jsou zbytecne slozite nebo malo oddeluji vrstvy abstrakce.
(pripadne spolehaji na vedlejsi efekty).

  * Napr. `CreateParticularOptionForRegistration()`, ale take
    `parseIntMultipleParameters()` nebo `parseInt()`.

  * Napr. `OptionSet.RegisterOption()`.

  * Napr. `HandleLongOptionIdentifier()`, `CheckMandatoryInstances()`, apod.

`-` Neni jasne, proc je parsovani prikazove radky rozdeleno mezi tridy
`ArgumentProcessor` a `Parser`, kdyz metoda `ParseCommandLine` v podstate
pouze vola metody tridy `ArgumentProcessor`.

`!` Chybi viditelny data flow. Top-level metody sice volaji jine metody, ale
nijak to nepomaha logickemu cleneni vrstev abstrakci.

Rada metod nema zadne parametry nebo vraci pouze `bool` jakozto informaci o
tom, zda ma volajici pokracovat v nejake akci ci nikoliv. Veskera manipulace s
daty se deje v nejnizsich urovnich a neni videt v metodach na vyssi urovni. Je
symptomaticke, ze jmeno u rady metod zacina `Handle*`, pricemz neni jasne, co
vlastne metoda dela, s jakymi daty a jak ovlivnuje stav.

`-` V rade metod je vystupni parametr uprostred, nikoliv posledni.

`-` Type switching na chybovych kodech. Bylo by vhodne reprezentovat
chyby vyjimkami a pripadnou specializaci chybovych hlasek delat v ramci
jednotlivych trid.

`-` Verejne API tridy nejsou `sealed`, ale nejsou navrzene pro dedicnost.


# Testy

`+` Knihovna integruje testy od kolegu a ma jeste radu vlastnich
parametrickych unit testu.


# Zdrojove kody

`!` Samostatne prikazy (v podrizenych vetvich ridicich prikazu) nejsou vzdy
uzavreny do bloku, podrizeny prikaz je casto na stejne radce jako ridici
prikaz.

  * Formatovani kodu by melo zduraznovat strukturu (nadrizeny/podrizeny kod)
    a ve stejnych situacich by melo vypadat pokud mozno stejne.

  * Je vhodne myslet i na to, ze pridani (pripadne odebrani) jednoho prikazu
    do bloku si vynucuje upravu formatovani.

Kod slozeny z radku jsou ty nasledujici je velmi spatne citelny.
```
if (!isOfSupportedTypesForParametrizedOption(typeof(TArgument)))throw new InvalidOperationException();
```

`-` Kod ve zdrojovkach je misty namackany na sebe a spatne citelny.

  * Bezne se mezi top-level elementy souboru/tridy, nebo mezi predchozi
    metodu a komentar k nasledujici metode davaji prazdne radky pro zlepseni
    citelnosti.

  * Mezi logickymi bloky v kodu metod je rovnez vhodne vkladat prazdne radky
    pro zlepseni citelnosti.

`~` V kodu je rada mist oznacenych `TODO`.

`~` Rada metod nedodrzuje zavedenou konvenci pro formatovani nazvu metod.

`~` V kodu se opakovane vyskytuji literaly ("=", "-", "--"), ktere maji
specificky vyznam a mely by byt idealne reprezentovany symbolem.


# Souhrnne hodnoceni

Zakladni idea API (tj. uzivatel si data uklada kam potrebuje) je rozumna,
provedeni vsak na nekterych mistech pokulhava a vede ke zbytecne slozitosti.
Zaroven se API nejevi jako prilis robustni vuci spatnemu pouziti, uz jen tim,
ze umoznuje nereagovat na chyby v konfiguraci.

Dokumentace je solidni, ale bylo by vhodne ji co nejvice integrovat se
zdrojovym kodem a ne ji drzet externe.

Jako jedni z mala mate vlastni testy, coz ocenuji.

Implementace a zdrojovky jsou slabsi, kod je spatne citelny a to i presto, ze
je videt urcita snaha rozclenit ho do mensich metod. Hlavni problem vidim v
tom, ze tridy nemaji chovani. To se da najit v podstate jen v jedne tride,
jejiz kod pracuje s daty jako s globalnimi promennymi (i kdyz se jedna o
instancni promenne tridy).

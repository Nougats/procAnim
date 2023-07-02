Projektname: Prozedural animierte Welt

Namen der Teammitglieder: Benjamin Dogic (200793)

Besondere Leistungen, Herausforderungen und gesammelte Erfahrungen während des Projekts. Was hat die meiste Zeit gekostet?

Am meisten Zeit hat zum einen das Erarbeiten der Bewegung für die Spinne gebraucht, und das Arbeiten an dem Wurm sowie für den Skorpionschwanz.
Leider ist mir auch erst sehr spät aufgefallen, dass es tatsächlich nicht möglich ist, mit dem Chain IK Constraint durch die einzelnen Bones
zwischen Tip und Root durch zu iterieren. Dadurch wurde aus einer flüssigen Bewegung die geplant war mit Hilfe von linearer Interpolation durch
die einzelnen Bones von Tip zu Root stattdessen nur das "suchen und greifen" des Targets.
Wenn man es gescheit machen wollen würde, sollte man wahrscheinlich gar nicht das Animation Rigging Package verwenden, da dieses wie schon im 
Namen enthalten zum riggen von schon verfügbaren Animationen gedacht ist, um diese noch flüssiger und lebendiger zu machen, und nicht um damit
die Animation von Grund auf zu erstellen und generieren zu lassen.
In dem Projekt selber sind noch zwei weitere Komponenten des Animation Rigging Packages verwendet worden, Dampening Transform und Override Transform.
Dampening Transform lässt sich gut benutzen um die Rotation und Position eines Bones auf die weiteren zu übertragen, ohne selbst viel Aufwand inves-
tieren zu müssen.
Override Transform überschreibt den Transform auf dem die Komponente liegt mit einer Quelle.
Beide Transforms zusammen sind sehr gut um Schwärme von Tieren darstellen zu lassen. In meinem Beispiel wären das die Fische im Teich, oder die "Nudeln"
in der Luft.
Ein weiterer großer Zeitfresser war unter anderem auch der Fakt, dass ich für viele der benutzten Modelle die Rigs und Bones in Blender selbst setzen
musste. Daraus sind glaube ich auch einige komische Transforms entstanden, da mein Prozess des Riggings der Bones nicht konsistent war.
Gutes Beispiel hierfür wäre das Snake Objekt in der Szene. Daher da ich die Modelle für alles außer den "Nudeln" importiert habe waren die Standard Modelle
ab und an auch nicht richtig ausgelegt, was auch zu Problemen geführt hat.

Im Projekt enthalten sind drei Ordner, Project Laptop, Noodle und World. Das Projekt war in drei geteilt, da ich an verschiedenen Orten und PCs daran
gearbeitet habe. Auf dem Stand der Abgabe sollte aber alles wichtige im Ordner Project World eingeordnet worden sein.

Verwendete Assets, Codefragmente, Inspiration. Alles was Sie nicht selbst gemacht haben bitte unbedingt angeben.

Assets:
Für die Welt in der ich die einzelnen Komponenten vorgeführt habe wurde das Low Poly Nature Bundle genutzt von LMHPOLY, sowie die darin enthaltene Szene
Demo 01
Die einzelnen Modelle für Tiere habe ich leider nicht dokumentiert, sind aber alle von der Seite www.sketchfab.com
Das genutzte Spinnen Modell aus der Szene, ist das Spider Orange Asset aus dem Unity Asset Store.

Codefragmente und Inspirationen:
Hauptsächlich nur für die prozedurelle Animation der Spinne

https://www.youtube.com/watch?v=abrJ3LXjLzA
https://www.youtube.com/watch?v=e6Gjhr1IP6w
https://www.youtube.com/watch?v=ZZjgJfHjxXI
https://www.youtube.com/watch?v=AhywDyu0EGw
https://www.youtube.com/watch?v=miB4qah7n_A
https://www.youtube.com/watch?v=Wx1s3CJ8NHw
https://www.youtube.com/watch?v=eTERzR4Yu5U
https://www.youtube.com/watch?v=vKiqs_h1WXM

Link zu einem kurzen Video (ca. 60-120s), welches das Projekt und seine Features in Action zeigt

https://youtu.be/VszT76uRo8A



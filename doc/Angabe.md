---
disable-header-and-footer: true
table-use-row-colors: true
geometry: margin=2cm
---

## Anforderungen

| Anforderung                                            | Wird behandelt durch |               Status |
| ------------------------------------------------------ | :------------------: | -------------------: |
| EF Database First                                      |    wird scho passn   |            notwendig |
| UserControls (Events + Properties)                     |         [#6]         |          koa Problem |
| Mehrere Fenster: Controls mit Namen, MVVM              |     [#4][#7][#8]     |          koa Problem |
| CSV-Daten -> DB (eine Zeile = Daten mehrerer Tabellen) |         [#3]         |          koa Problem |
| Datenbank (get, update, delete, insert)                |       [#1][#2]       |          koa Problem |
| MVVM: ViewModel                                        |         [#7]         |      wird scho passn |
| Treeview                                               |         [#5]         | miass ma se onschaun |
| MVVM: Behaviors (Moodle)                               |                      |       lernan ma erst |
| UnitTest (wenn noch Zeit)                              |                      |       lernan ma erst |
| MVVM: Dependency Injection                             |                      |       lernan ma erst |

## Chinook-Datenbank

Musik: Artists, Albums, Tracks, Playlist

Vertrieb: Rechnungen, Rechnungsitem, Kunden, Employees

## Szenarien [#4]

#### Vertriebs-Verwaltung

Benutzer ist ein Employee ("Identität" (in Form eines UserControls? [#6]) wechselbar [#1]), kann Rechnungen für >= 1 Alben an Customer ausstellen

#### Playlist-Verwaltung

Benutzer kann mehrere Playlists erstellen, ändern und löschen [#2]. Playlists bestehen aus einem Namen und >= 0 Songs. Import und Export per CSV [#3] und Persistierung in der Datenbank möglich. Anzeige der Songs einer Playlist per Treeview [#5].

Kunnt ma mid MVVM mochn, miass ma schaun, wie ma do generell dan [#7].

#### Portfolio-Verwaltung

Benutzer als Artist kann Alben sowie die zugehörigen Tracks erstellen.

Kunnt ma mid namenbasiertem Access mochn [#8].

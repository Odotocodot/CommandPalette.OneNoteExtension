<div align="center">
<img alt="Extension Logo" src="OneNoteExtension/Assets/Icons/logo.svg" height="100"/>

# OneNote for Command Palette

</div>

## Main Features

- [Search OneNote pages](#search-onenote-pages)
- [OneNote Explorer](#onenote-explorer):
  - View your OneNote structure without opening OneNote
  - Create OneNote pages, sections, section groups and notebooks.
- View recently edited pages
- [Create a _Quick Note_](#create-a-quick-note)

> [!IMPORTANT]
>
> - Requires OneNote to be installed.
> - For search to function, requires the Windows Search to be enabled.
> - Creating a *Quick Note* requires a [Quick Note Section][quickNotesSection] to be set.

### Search OneNote Pages

![search page png](doc/default_search.png)

> [!NOTE]
> You can include bitwise operators like `AND` or `OR` (they must be uppercase) in your search. E.g. `hello there AND general kenobi`.

### OneNote Explorer

![onenote explorer png](doc/onenote_explorer.png)

TODO  Search in item, Can also change filter to search differently

#### Creating items

![creating items png](doc/create_new_item.png)

TODO
For now, to create an item in a parent requires
Leads to a the plugin displaying a similar view as show in [aawdawdwadwa](#create-a-quick-note) depending on wether you are creating a OneNote notebook, section group, section or page.

### Create a Quick Note

![create a quick note](doc/create_quick_note.png)

Creates a OneNote page at your [_Quick Notes Section_](quickNotesSection). To create a page in a specific section use the OneNote Explorer.

## TODO

- [ ] Publish Package on:
  - [ ] MS store and/or
  - [ ] Winget

[quickNotesSection]: https://private-user-images.githubusercontent.com/48138990/557685458-eac29b9f-9c92-47bc-af32-6e38c5435a2d.png?jwt=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTUiLCJleHAiOjE3NzI2NjI3MTMsIm5iZiI6MTc3MjY2MjQxMywicGF0aCI6Ii80ODEzODk5MC81NTc2ODU0NTgtZWFjMjliOWYtOWM5Mi00N2JjLWFmMzItNmUzOGM1NDM1YTJkLnBuZz9YLUFtei1BbGdvcml0aG09QVdTNC1ITUFDLVNIQTI1NiZYLUFtei1DcmVkZW50aWFsPUFLSUFWQ09EWUxTQTUzUFFLNFpBJTJGMjAyNjAzMDQlMkZ1cy1lYXN0LTElMkZzMyUyRmF3czRfcmVxdWVzdCZYLUFtei1EYXRlPTIwMjYwMzA0VDIyMTMzM1omWC1BbXotRXhwaXJlcz0zMDAmWC1BbXotU2lnbmF0dXJlPTE5MDlhYzZkOTA4MzI1OTU0M2E0ZDU4MjZlOGJlMzlmMTNlOTNiYzg0MmJiNzg0NjM0ZWNhOWM5NzE4MmJmODImWC1BbXotU2lnbmVkSGVhZGVycz1ob3N0In0.ZNf_v1WpTSlCQ22eEyn21jV7oA8wOepjVtOVrhnCWbc
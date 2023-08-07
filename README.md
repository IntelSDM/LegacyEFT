# EftHag
<br /> **Credits**
<br /> The Overlay Uses An Old Build Of A Direct2D From Michel Pi [GameOverlay .Net](https://github.com/michel-pi/GameOverlay.Net).
<br /> The Renderer For The Overlay Is [SharpDx](https://github.com/sharpdx/SharpDX)
<br />
<br /> **Introduction**
<br /> This cheat is built similarly to my other project Auft. It has a .net overlay with an independent thread to render while using mono behaviours.
Although there are similarities between the projects EftHag has quite a few important optimization fixes. This involves removing as much from the unity main thread as possible. In unity calls from Update or any constant callback made for unity goes onto the unity main thread. In Eft this thread is really slow for some reason probably stemming from bad game developers. I only initialize the my code in a Start or Awake callback and then initialize Coroutines to let .net and windows manage threads for my code from there which saves a lot of frames. In the old Auft project there was a lot of code being ran in the renderer when it didn't need to be, withn this we use objects for everything and calculate everything among different threads to the renderer which allows us to maintain a constant 133fps.
<br />
<br />**Functionality**
<br />Aimbot - Hitscan aimbot custimizable to all enemy types 
<br />Esp - There is esp for absolutely every entity in the game and customizability for every enemy
<br />Menu - This includes an entire menu framework i have coded from scratch
<br />Config - Object based json config system
<br />Colours - Colours can be modified for everything
<br />Hooks - Plenty of hooked features such as silent aim, sessionid spoofing, instant search, no flash

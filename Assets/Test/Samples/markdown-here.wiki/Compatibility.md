Info about the places where *Markdown Here* works, including limitations and workarounds.

If you find a new problem (or improvement!), or if you find a site that (almost) works, or if you have workflow suggestions, please edit this wiki page, or create a [Github issue](https://github.com/adam-p/markdown-here/issues), or post to the [MDH Google Group](https://groups.google.com/forum/#!forum/markdown-here).


* [The "Works Great" Bucket](#the-works-great-bucket)
  * [Gmail](#gmail)
  * [Inbox By Google](#inbox-by-google)
  * [Thunderbird](#thunderbird)
  * [Google Groups](#google-groups)
  * [Evernote](#evernote)
  * [Blogger](#blogger)
  * [Google Sites](#google-sites)
  * [Outlook.com/Hotmail](#hotmail)
  * [Yahoo email](#yahoo)
  * [Wordpress](#wordpress)
  * [Freshdesk](#freshdesk)
  * [Fastmail](#fastmail)
  * [ProtonMail](#protonmail)
* [Postbox](#postbox)
* [Facebook](#facebook)
* [Google Hangouts](#google-hangouts)
* [Tumblr](#tumblr)
* [Squarespace](#squarespace)
* [Blackboard Learn](#blackboard-learn)
* [Editor Tools](#editor-tools)
  * [TinyMCE](#tinymce)
    * [Pasting vs. Typing](#pasting-vs-typing)
  * [CKEditor](#ckeditor)
  * [Aloha Editor](#aloha-editor)
  * [Redactor](#redactor)
  * [Hallo](#hallo)
  * [wysihtml5](#wysihtml5)
  * [bootstrap-wysihtml5](#bootstrap-wysihtml5)


<a name="the-works-great-bucket"/>

### The "Works Great" Bucket

<a name="gmail"/>

#### Gmail

First-class client. Works great. See open issues [here](https://github.com/adam-p/markdown-here/labels/gmail).

<a name="thunderbird"/>

#### Thunderbird

First-class client. Works great. See open issues [here](https://github.com/adam-p/markdown-here/labels/thunderbird).

<a name="inbox-by-google"/>

#### Inbox By Google

Comparable to Gmail. Except the MDH button doesn't enable while focus is in the "quick compose" box ([issue #221](https://github.com/adam-p/markdown-here/issues/221)) -- but it the hotkey and context menu still work there.

<a name="google-groups"/>

#### Google Groups

*Markdown Here* works with Google Groups posts. You can use it in the GG rich compose box, or when you're posting via email. One caveat: Digest emails strip all styling. 


<a name="evernote"/>

#### Evernote

[A user discovered](https://github.com/adam-p/markdown-here/issues/30#issuecomment-8119861) that Markdown Here works in the [Evernote](https://www.evernote.com) web interface. It works because Evernote uses a somewhat backlevel version of [TinyMCE](#tinymce).


<a name="blogger"/>

#### Blogger

Works moderately well.

There was a styling fix for Blogger in v2.9.0, so make sure you [reset your Primary Styling CSS](Troubleshooting#getting-the-latest-primary-styling-css).

There is currently a problem with editing a draft that was saved rendered: it's no longer possible to unrender, and the styling looks wrong (in the edit view). It was discovered that  Blogger is mangling some attributes -- like `style` and `title` (which is used for storing the original MD). I created a separate extension to work around the problem: https://github.com/adam-p/wysiwyblogger

Other caveats:

* Some syntax highlighting themes with dark backgrounds don't seem to show up properly. There's an example at the bottom of [this post](http://adampersand.blogspot.ca/2013/10/mdh-test-better-blogger.html).

Thanks to [lambdaalice](https://github.com/lambdalice) for originally [reporting](https://github.com/adam-p/markdown-here/issues/89) that MDH worked with Blogger and for detailing the previous bad behaviour.


<a name="google-sites"/>

#### Google Sites

[Google Sites](https://sites.google.com) seems to use an editor similar to Gmail and Google Groups.

You can see some [test pages here](https://sites.google.com/site/markdownheretest/).


<a name="hotmail"/>

#### Outlook.com/Hotmail

Works very well. 

Caveats: 

* Email received from Yahoo does not display with properly separated paragraphs. (Hotmail strips styling off `<p>` and `<div>` tags, and Yahoo uses the latter for paragraphs.)
* No reply exclusion.
* No forgot-to-render detection.


<a name="yahoo"/>

#### Yahoo email

Works very well. 

Caveats:
* Some reply exclusion problems.
* No forgot-to-render detection.


<a name="wordpress"/>

#### Wordpress

As of version 2.9.0, earlier problems are fixed and [Wordpress](http://wordpress.com/) is working well. Part of the fix came from changes to the default styling. If you've never customized your CSS, you should click the "Reset to Default" button for the "Primary Styling CSS". If you have customized your CSS, you can [take a look at the default CSS](https://github.com/adam-p/markdown-here/blob/master/src/common/default.css) and decide what to take.

Caveats and tips:
* The Wordpress "Preview" button is your friend. What you see there (but not so much in the edit box) is what you get.
* After rendering, inline code appears in a non-monospace font, but it is correctly monospace in the preview and in the finished post.
* The section below on [Pasting vs. Typing](#pasting-vs-typing) applies here.
* Like with the Yahoo rich controls, if the paragraph type combo is clicked, focused-element finding gets busted and Markdown Toggle stops working. See [issue #16](https://github.com/adam-p/markdown-here/issues/16).
* In Chrome, MDH doesn't work in the drop-down post editor, due to cross-origin `iframe` restrictions. Click the "Pop-out" link and MDH will become usable in the separate window. Alternatively, use the full editor in the admin interface. (See [issue #124](https://github.com/adam-p/markdown-here/issues/124).)
* It can be disorienting when composing to see extra (paragraph) space when hitting `Enter` once -- it looks like a blank line, but it isn't, and it's still only a single `Enter` for Markdown purposes. Some users find this disorienting, and there's [some discussion here](https://github.com/adam-p/markdown-here/issues/123) about a possible different approach to composing and styling.

Thanks to [Sina Iravanian](https://plus.google.com/116422808039109985732/posts) for originally discovering that MDH works with Wordpress.

Check out a [test post](http://adampritch.wordpress.com/2013/10/05/markdown-here-test-post/).


<a name="freshdesk"/>

#### Freshdesk

[@rfay](https://github.com/rfay) [reports](https://github.com/adam-p/markdown-here/issues/227) that Markdown Here works great with the [Freshdesk](http://freshdesk.com/) ticket editor.


<a name="fastmail"/>

#### Fastmail

[@alexandru](https://github.com/alexandru) and [@leviwheatcroft](https://github.com/leviwheatcroft) reported (via issues) that Markdown Here works with the [Fastmail](https://www.fastmail.com) webmail service.

Caveats:

* Fastmail defaults to plaintext editing. Switch to rich edit. (You can make the switch permanent in the settings.)
* Fastmail's default rich signature has a broken signature separator -- it uses `--` (dash-dash) instead of `-- ` (dash-dash-space). You can modify your signature in Fastmail's Settings/Accounts.


<a name="protonmail"/>

#### ProtonMail

[ProtonMail](https://protonmail.ch/) is an end-to-end encrypted email service.

[Antonio Gil reported](https://groups.google.com/forum/#!topic/markdown-here/v2UVY_RR5LY) that Markdown Here works very well with it.


<a name="postbox"/>

### Postbox

[Postbox](http://www.postbox-inc.com/) is a non-free desktop email client based on Thunderbird, and user [markgoodson](https://github.com/markgoodson) requested that *Markdown Here* [support it](https://github.com/adam-p/markdown-here/issues/30). The Mozilla extension now works with it, but with some major caveats:

* There's no options page. However, you can open the "Config Editor" from the Preferences dialog and copy/paste options from Firefox's or Thunderbird's equivalent config editor (`about:config` in Firefox, "Config Editor" in Thunderbird). *Note*: Make them all string values. 
  * I just couldn't figure out how to open a tab with the options page. This will probably require the assistance of Postbox or someone familiar with developing for it.


<a name="facebook"/>

### Facebook

Facebook "Notes" feature users TinyMCE as its editor, so MDH kinda works. But there are caveats.

* The Preview function doesn't play well with Markdown Here. If write some stuff, then MDH-render, then preview, and decide to go back to edit some more... you can't un-render back to Markdown. (It has stripped out the special MDH stuff.)
* Pasting plaintext seems to lose newlines completely. This is weird and annoying, but actually helps prevent TinyMCE's [Pasting vs. Typing](#pasting-vs-typing) confusion.
* Only the formatting types presented on the Notes' formatting toolbar are supported, at all. If you put a Markdown code block into the edit box and MDH-render it, it will look fine in the editor, but if you preview or publish it, you'll see that it's been stripped out.
* Italics doesn't work in preview/publish. The italicize button on the Notes' formatting toolbar behaves that same broken way as well. `Ctrl+I` works, though. It seems that Notes doesn't allow `<em>` but does allow `<i>`.


<a name="google-hangouts"/>

### Google Hangouts

The [Google Hangouts](http://www.google.com/hangouts/) web interface supports limited rich editing -- [bold, italics, and underline](https://support.google.com/hangouts/answer/3112005). Markdown Here can format bold and italics in the interface, with caveats:

* The surrounding `<p>` element makes the little compose box look oddly expanded after rendering. There's no ill effect, though.
* In Chrome, the chat box has to be popped out of the Gmail page in order for MDH to work. This is because (I think) MDH is loaded into the Gmail page and there are cross-origin restrictions between the top `mail.google.com` page and the `talkgadget.google.com` `iframe` where the chat box is. (See [issue #124](https://github.com/adam-p/markdown-here/issues/124).)
  * Firefox doesn't have this problem. Opera surely does. Not sure about Safari.

<a name="tumblr"/>

### Tumblr

[Tumblr](http://tumblr.com) is a blogging-ish site and service. 

It looks likes Tumblr strips out inline styles, so none of the styling in MDH will be applied. The formatting types supported by the Tumblr rich formatting toolbar seem to work fine: bold, italics, links, ordered and unordered lists, blockquotes, images. Code blocks will be left intact, but with no syntax highlighting (and inline code is completely stripped). If you work within those constraints, it's usable. [Example post here](http://adam-p.tumblr.com/post/63976503036/markdown-here-paste-test). And it's no worse than Tumblr's optional built-in Markdown renderer, as can be [seen here](http://adam-p.tumblr.com/post/64197896520/mdh-test-using-tumblrs-md-rendering).

Tumblr also has the option to use a plaintext editor that allows the input of raw HTML. If MDH's output is pasted into that, it actually [looks pretty good](http://adam-p.tumblr.com/post/64198046456/mdh-test-pasted-html-into-plaintext-editor). There is an outstanding feature request ([#43](https://github.com/adam-p/markdown-here/issues/43)) for the ability to render to raw HTML text that would enable this workflow.


<a name="squarespace"/>

### Squarespace

[Squarespace](http://squarespace.com) is a non-free site creation and hosting service. 

MDH ought to work, but looks pretty bad. Needs to be investigated.


<a name="blackboard-learn"/>

### Blackboard Learn

[Blackboard Learn](https://en.wikipedia.org/wiki/Blackboard_Learn) ([official site](http://www.blackboard.com/)) is a "virtual learning environment and course management system". 

MDH works well in at least some areas of the site. Details can be found in issue [#293](https://github.com/adam-p/markdown-here/issues/293).


<a name="editor-tools"/>

### Editor Tools

There's a whole class of rich editors for use in web pages that Markdown Here woks with.

<a name="tinymce"/>

#### TinyMCE

[TinyMCE](http://www.tinymce.com/) is an open source "web based Javascript HTML WYSIWYG editor control". It's used as a rich edit control by these sites (at this time), [among others](http://www.tinymce.com/enterprise/using.php): Evernote, Wordpress, Facebook (Notes). It seems to have lots of customization options for changing the way it works and what formatting it allows and doesn't. Individual sites of reasonable importance with idiosyncratic behaviour should still have a separate section on this page.

TinyMCE inserts `<p>` elements on every `Enter` keypress. Prior to *Markdown Here* version 2.9.0, this would cause extra blank lines in code blocks, break tables, and probably other bad things. MDH's `<p>`-vs-`<br>` detection basically fixes this. (Although it will looks odd when writing, since there's more space between newlines than one expects when writing Markdown.)

<a name="pasting-vs-typing"/>

##### Pasting vs. Typing

An annoying TinyMCE oddity is that pasting plain-text is *not* the same as typing. When pasting multi-line text, line breaks (`<br>`) are inserted; when typing, paragraphs (`<p>`) are created. 

This can cause Markdown Here to be confused about what rendering method to use. So, generally speaking, it's best to not mix and match multi-line pasted and typed Markdown. And if you do paste some stuff, try to render it separately using a selection.

<a name="ckeditor"/>

#### CKEditor

[CKEditor](http://ckeditor.com/) seems to be exactly the same (for our purposes) as TinyMCE -- both in what it is and how it behaves. So read that section.


<a name="aloha-editor"/>

#### Aloha Editor

[Aloha Editor](http://www.aloha-editor.org/) is also an open source rich web editor. Like TinyMCE it inserts `<p>` elements, but unlike TinyMCE it doesn't switch to `<br>` when pasting.  Markdown Here seems to render it quite well.

<a name="redactor"/>

#### Redactor

[Redactor](http://imperavi.com/redactor/) is a rich editor, like TinyMCE (but not open source). Like TinyMCE it inserts `<p>` elements, but unlike TinyMCE it doesn't switch to `<br>` when pasting. Markdown Here seems to work pretty well with it, with caveats:

* Code blocks have a background colour and an outer border. This can probably be defeated with site-specific rules and `!important`. See the default Primary Styling CSS for an example used with Wordpress.
* Some other styling stuff.

<a name="hallo"/>

#### Hallo

[Hallo](http://hallojs.org/) is a simple rich editor. Markdown Here seems to work reasonably well with it.

<a name="wysihtml5"/>

#### wysihtml5

[wysihtml5](https://github.com/xing/wysihtml5) is also an open-source rich editor. It inserts `<br>` on each `Enter`. Markdown Here seems to work pretty well with it.

<a name="bootstrap-wysihtml5"/>

##### bootstrap-wysihtml5

[bootstrap-wysihtml5](http://jhollingworth.github.io/bootstrap-wysihtml5/) is based on wysihtml5 for use with the Twitter Bootstrap web toolkit. Markdown Here works well with it as well.
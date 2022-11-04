---
description: Text/video learning resources for everyone!
---

# Tutorials

Apart from the [Getting started](getting-started.md) video series, we offer a wide variety of tutorials that address different common mechanics.

{% hint style="success" %}
Do you know of a video tutorial that's not included? Send us an [email](mailto:hello@gamecreator.io) and we'll evaluate it!
{% endhint %}

## Showcase

You can download example scene to get more an idea what StoryTime offers.

![Examples created in Unity](../.gitbook/assets/storytime\_package\_overview.png)

## &#x20;Tips & Tricks

{% embed url="https://youtu.be/opI2wZSwVAA" %}

### Localization

If you want to use dialogue you have to make use of the localization package.&#x20;

* Go to `Window > Asset Management > Localization tables`&#x20;

![](../.gitbook/assets/localization\_start\_screen.png)

* Create `Localization Settings.asset` file inside the `Localization` folder.
* Select `Locale Generator` and choose which languagues you want to support and save it under `Localization > Locales`.
* Select `New Table Collection` and type in `Dialogues` or something that will represent your dialogue data. Click on `Create String Table Collection` and save it under `Localization  > Dialogues`

![](../.gitbook/assets/new\_localized\_tables.png)

* In the inspector view you will see an empty list with`Extensions`. Click on `+` to add a new extensions and click on `Json Extension.`

![](../.gitbook/assets/string\_table\_inspector.PNG)

* Drag or select the config file for the json service provider.
* Under `Select file` Choose `dialogues`.&#x20;
* Click the `+`, choose `Add Default Fields`and click `Pull`.

![](../.gitbook/assets/string\_table\_inspector\_pull.png)




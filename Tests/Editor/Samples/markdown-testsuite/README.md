https://github.com/karlcow/markdown-testsuite

# Colophon - October 1st, 2014

This project was initiated to provide a test suite for Markdown markup, and eventually create a specification from this test results. A part of of the community has started a new endeavor which seems to get traction as [CommonMark](https://github.com/jgm/stmd). **We are then closing this project** and encourage you to contribute to CommonMark. 

The most interesting part of this project would not have been possible without the dedication of [Ciro Santilli](https://github.com/cirosantilli) @cirosantilli. So big applause and thank you to him.


The rest is kept around for archives and references.

# Markdown Test Suite

Inspired by questions on [W3C Markdown Community Group](http://www.w3.org/community/markdown).

Pull Requests are welcome. See the [CONTRIBUTING Guidelines](https://github.com/karlcow/markdown-testsuite/blob/master/CONTRIBUTING.md)

## Design goals

- Comprehensive.
- Small modularized tests.
- Easy to run tests using any programming language. In particular, data representations must have an implementation on all major languages.
- Develop a consensus based markdown specification at [markdown-spec.html](markdown-spec.html). Visualize it [here](http://htmlpreview.github.io/?https://github.com/karlcow/markdown-testsuite/blob/master/markdown-spec.html).

## Test Scripts

Markdown Test Suite already includes tests for many important markdown engines.

To see what the scripts do run:

    ./cat-all.py -h
    ./run-tests.py -h

To configure the scripts do:

	cp config_local.py.example config_local.py

and edit `config_local.py`. It is already gitignored.

A `Vagrantfile` is provided with a provision script that installs all installable engines.

Sample output from `run-tests.py`:

    blackfriday   |......FFF..............FF...F....................................F....................................|   0.93s  102    7   6%
	gfm           |FF.....F.....F...FFFF..F....F........................FFFF...FF...............FF...F.......F...........| 262.88s  102   20  19%
    hoedown       |..............................F.................................................F.....................|   0.36s  102    2   1%
	kramdown      |......FFF.....FF.......FF.......FF.FFFFFFFFFFFFF.......................F..............................|  30.69s  102   23  22%
    lunamark      |FFFFFF.F.FFFFFFFFFFFF.F.....FFFF..FF............FFFFF..FFFFFFFFFF..............F...FFFFFFFFFFF........|   1.58s  103   53  51%
    markdown_pl   |.....................FFFF...............................F...............F.............................|   2.56s  102    6   5%
    markdown2     |......................................................................................................|   5.39s  103    0   0%
    marked        |..............F.................FFFFFFFFFFFFFFFF......................F.F.............................|   6.22s  102   19  18%
    maruku        |......FFF......F.......F.FFF...............................................FF.........................|  37.02s  102   10   9%
    md2html       |.........................FFF...F............................................FF........................|   7.42s  102    6   5%
    multimarkdown |......FFF....FF...F............FFF.FFFFFFFFFFFFF.....FFFF.........F...................................|   0.58s  102   27  26%
    pandoc        |FF...........F.F.FFFF..FFFFF....FF.FFFFFFFFFFFFF.....FFFF.....FF............FFF.FFF..................F|   1.11s  102   41  40%
    peg_markdown  |.................................................................F....................................|   0.46s  102    1   0%
    rdiscount     |.......F.........................................................F....................................|  25.19s  102    3   2%
    redcarpet     |......................................................................................................|  21.49s  102    0   0%
    showdown      |.......................................................................F..............................|   6.85s  102    1   0%

	Extensions:

    blackfriday   |F..|  0.04s    3    1  33%
	gfm           |F.|   2.37s    2    1  50%
    hoedown       |.|    0.00s    1    0   0%
    lunamark      |F.F|  0.05s    3    2  66%
    kramdown      |..|   0.62s    2    0   0%
    markdown_pl   ||     0.00s    0    0   0%
    markdown2     |F.|   0.14s    2    1  50%
    marked        |F.|   0.13s    2    1  50%
    maruku        |F..|  1.06s    3    1  33%
    md2html       |F.|   0.14s    2    0   0%
    multimarkdown |F.|   0.01s    2    1  50%
    pandoc        |F.|   0.02s    2    1  50%
    peg_markdown  |...|  0.02s    3    0   0%
    rdiscount     |F..|   0.74s    3    1  33%
    redcarpet     |..|   0.42s    2    0   0%
    showdown      |F..|  0.20s    3    1  33%

Where `F` indicates a failing test.

## Other Noticeable Test Suites

We haven't been the first test suite effort. Some projects have maintained their own test suite for a long time. Hopefully we can reach a state where people agree on the terms of what should be a good test suite for all developers.

- [Original test suite](http://daringfireball.net/projects/downloads/MarkdownTest_1.0.zip)
- [PHP markdown test suite:](https://github.com/michelf/mdtest/tree/master/Markdown.mdtest)
- [Markdown-js test suite](https://github.com/evilstreak/markdown-js/tree/master/test/features)
- [Python markdown 2 test suite](https://github.com/trentm/python-markdown2/tree/master/test/tm-cases)
- [multimarkdown test suite](https://github.com/fletcher/MMD-Test-Suite)
- [John MacFarlane's Standard Markdown spec](https://github.com/jgm/stmd)

In addition we should note the [wonderful work](http://johnmacfarlane.net/babelmark2/) made by John Mac Farlane. The Web service output the differences in between the different markdown implementations. It helps a lot when searching on the most common output.

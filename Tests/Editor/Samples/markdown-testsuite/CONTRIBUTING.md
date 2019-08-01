# Contributing to Mardown Test Suite

1. [Getting Involved](#getting-involved)
2. [Discussion](#discussion)
3. [How To Report an Issue](#how-to-report-an-issue)
4. [Editing Markdown Specification](#editing-markdown-specification)
5. [Test Suite Guidelines](#test-suite-guidelines)
6. [Pull Request Guidelines](#pull-request-guidelines)

## Getting Involved

There are a number of ways to get involved with the development of the Markdown Test Suite. If you are a first time contributor to an open source project, you can still add value to this project. You may identify grammar issues, write prose, examples or documentation for the project. You may flag some errors and open new issues.

Read all bits of this document and that will guide you on how to participate.

## Discussion

### Mailing List

The Markdown Test Suite has been started a little bit after the start of the [Markdown Community Group](http://www.w3.org/community/markdown/). You may want to discuss about Markdown and this project on the [group mailing-list](http://lists.w3.org/Archives/Public/public-markdown/).

## How to Report an Issue

Don't be afraid to add details to your issue. The more context you give, the better for people participating to the project. Make a short summary, add a description with the context and give links to places which will help to understand.

## Editing Markdown Specification

There is much needed work on the [Markdown Specification](http://htmlpreview.github.io/?https://github.com/karlcow/markdown-testsuite/blob/master/markdown-spec.html). It doesn't require strong competences in coding. Be systematic in the markup and follow the way it is already organized. Each time you added a new atomic section, create a commit for this specific part.

## Test Suite Guidelines

When proposing new tests, make sure they do not already exist in the current test suite. You will notice that there is a separate section dedicated to the specific extensions that some markdown generators have created. Keep them separate. We want to keep the core clean and close to the original specification of Markdown.

If you are not sure, create an issue.

### Markdown Extensions

Markdown extension tests are accepted. Use the following rules:

- place all extension tests under the `test/extensions/ENGINE` directory with filename equal to the feature name
- for each `ENGINE` and add a `README.markdown` under the engine directory with a link to its specification

where `ENGINE` is either of:

- a command line utility. E.g. `pandoc`, `kramdown`.
- a programming API. E.g. `Redcarpet::Markdown.new()`.
- a programmatically accessible web service. E.g.: GFM via the GitHub API.
- a non programmatically accessible web service. E.g.: Stack Overflow flavored markdown.

To avoid test duplication for common features, use the following rules:

- if file `tests/extensions/ENGINE/FEATURE.md` is empty or not present, use `tests/extensions/FEATURE.md` instead
- if file `tests/extensions/ENGINE/FEATURE.out` not present, use `tests/extensions/FEATURE.out` instead
- for a given `ENGINE`, only features which have either a `.md` or a `.out` are implemented by that engine.
- in case of different outputs for a single feature input, keep directly under `extensions/` only the output case that happens across the most engines

**version**

Only the latest stable version of each `ENGINE` is considered.

**options**

The IO behavior tested is for engine defaults, without any options (command line options, extra JSON parameters, etc.) passed to the engine.

**external state**

Tests that use external state not present in the markdown input shall not be included in this test suite.

For example, what GFM `@user` user tags render to also depends on the state of GitHub's databases (only render to a link if user exists), not only on the input markdown.

#### Examples

GFM and the "fenced code block" use the following file structure:

    tests/extensions/gfm/README.markdown
    tests/extensions/gfm/fenced-code-block.md
    tests/extensions/gfm/fenced-code-block.out

where `README.markdown` contains:

    https://help.github.com/articles/github-flavored-markdown

To avoid duplication with other engines, if the input / output (normalized to DOM) is the same across multiple engines use:

    tests/extensions/gfm/fenced-code-block.md       [empty]
    tests/extensions/fenced-code-block.md
    tests/extensions/fenced-code-block.out

If the input is the same, and outputs are DOM different, but represent the same idea (e.g. both represent computer code) use:

    tests/extensions/gfm/fenced-code-block.md       [empty]
    tests/extensions/gfm/fenced-code-block.out
    tests/extensions/fenced-code-block.md
    tests/extensions/fenced-code-block.out

#### New Extensions

Tests are modular, and if you want to test a new engine for your project, that should be easy to do.

If you feel that the new engine is reasonably popular, please send a pull request adding it.

## Commits and Pull Requests

* Keep your commits clean and commented
* Do not mix in one commit different things. Keep modifications related.
* Do not mix everything in one giant pull request. Create separate pull requests for different topic. That will help to have a more meaningful debate about the pull requests and will help to review the code.
* If it relates to a specific issue, add the number of the issue in the commit message.

Thanks for Contributing

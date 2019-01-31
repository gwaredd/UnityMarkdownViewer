#------------------------------------------------------------------------------

_     = require "lodash"
chalk = require "chalk"
yargs = require "yargs"
axios = require "axios"
path  = require "path"
fs    = require "fs-extra"

log   = require "./libs/log"
utils = require "./libs/utils"


#------------------------------------------------------------------------------
# command line args

version     = '1.0.0'
description = "Create an asset store release project"
options     =
    out:
      alias       : 'o'
      description : 'output directory'
      requiresArg : true
      required    : true

argv = yargs
  .usage  chalk.cyan( "Usage:" ) + chalk.yellow " $0 [options]" + chalk.white "\n\n" + description
  .help   'help'
  .alias  'help', [ 'h', '?' ]
  .version version
  .alias  'version', [ 'v' ]
  .options options
  .argv


#------------------------------------------------------------------------------

log.info "Output directory: #{chalk.cyan argv.out}"

root = path.join __dirname, "../.."
dst  = path.join root, argv.out

toCopy = [
  "ProjectSettings",
  "Packages",
  "Assets/AssetStoreTools",
  "Assets/MarkdownViewer",
  "Assets/Editor Default Resources"
]

toDelete = [
  "Assets/MarkdownViewer/Editor/MarkdownViewerAssembly.asmdef"
  "Assets/MarkdownViewer/Editor/MarkdownViewerAssembly.asmdef.meta"
]

try

  log.info "Removing #{chalk.cyan dst}"
  fs.removeSync dst

  dir = "#{dst}/Assets"
  log.info "Creating #{chalk.cyan dir}"
  fs.ensureDirSync dir

  for dir in toCopy
    log.info "Copying #{chalk.cyan dir}"
    fs.copySync "#{root}/#{dir}", "#{dst}/#{dir}"

  for f in toDelete
    log.info "Deleting #{chalk.cyan f}"
    fs.removeSync "#{dst}/#{f}"
  
  log.warn "TODO: copy markdig.dll"


catch err

  log.error err


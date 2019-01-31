#------------------------------------------------------------------------------

_     = require "lodash"
chalk = require "chalk"
yargs = require "yargs"
axios = require "axios"
path  = require "path"
glob  = require "glob"
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
# utils

root = ""

relative = (f) -> f.replace root, ""

remove = (file) ->
  log.info "#{chalk.red 'Deleting'} #{relative file}"
  fs.removeSync file

copy = (src,dst) ->
  log.info "#{chalk.cyan 'Copying'} #{relative src}"
  fs.copySync src, dst


#------------------------------------------------------------------------------

try

  log.info "#{chalk.green 'Output'} #{argv.out}"

  root = path.join( __dirname, "../.." ).replace /\\/g, "/" 
  dst  = path.join( root, argv.out ).replace /\\/g, "/" 

  remove dst

  copy "#{root}/#{dir}", "#{dst}/#{dir}" for dir in [
    "ProjectSettings",
    "Packages",
    "Assets/AssetStoreTools",
    "Assets/MarkdownViewer",
    "Assets/Editor Default Resources"
  ]

  remove f for f in glob.sync "#{dst}/**/*.asmdef*"

  copy "#{root}/Tools/res/Markdig_35.dll", "#{dst}/Assets/MarkdownViewer/Editor/Markdig.dll"

  log.success "Done"


catch err

  log.error "#{err}"


#------------------------------------------------------------------------------

log = require "./log"


#------------------------------------------------------------------------------
# string.format

if not String.prototype.format

  String.prototype.format = ->
  
    # TODO: colour replacements
    # TODO: escape characters

    args = arguments

    this.replace /{(\d+)(:([\d-]+))?}/g, (match, arg, x, pad) ->

      return match if typeof args[arg] == 'undefined'
      return String args[arg] if typeof pad == 'undefined'

      padding = parseInt pad

      if padding < 0
        return String( args[arg] ).padStart -padding, ' '
      else
        return String( args[arg] ).padEnd padding, ' '


#------------------------------------------------------------------------------

module.exports =
  die: (msg) -> log.fatal msg; process.exit 1


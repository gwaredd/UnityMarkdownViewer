#------------------------------------------------------------------------------

utils = require "../libs/utils"
should = require('chai').should()


#------------------------------------------------------------------------------

describe 'Example Test', ->
  it 'Tests a thing', ->
    [1,2,3].indexOf(4).should.equal -1


#------------------------------------------------------------------------------

describe 'String Format', ->

  it 'Basic formatting', ->

    # basic
    "num={0}".format(1).should.equal "num=1"
    "{3},{2},{1},{0}".format('a',3,4.1,true).should.equal "true,4.1,3,a"

    # bad indicies
    "num={100}".format(1).should.equal "num={100}"
    "num={-1}".format(1).should.equal "num={-1}"

  it 'Padding', ->
    "a{0:12}b".format('hello').should.equal  "ahello       b"
    "a{0:-12}b".format('hello').should.equal "a       hellob"
    "a{0:4}b".format('hello').should.equal  "ahellob"

    "a{0:4}b".format(12345678).should.equal  "a12345678b"
    "a{0:12}b".format(12345678).should.equal  "a12345678    b"
    "a{0:-12}b".format(12345678).should.equal  "a    12345678b"

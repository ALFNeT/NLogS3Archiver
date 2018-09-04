# NLogS3Archiver

## What in tarnation is this?

It's just me learning how to try to extend NLog, here are some of the things I'm trying to achieve:

* Add compression formats to the NLog archiver, currently looking into GZip and BZip2 for Apache Spark compatability.
* Archived files upload to AWS S3
* Investigate Parquet output format for the logs.
* Investigate if it's possible to extend the native File target to implement the above features.

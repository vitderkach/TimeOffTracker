"use strict";
var gulp = require("gulp"),
    path = require("path"),
    less = require("gulp-less");

var paths = {
    webroot: "./wwwroot/"
};

gulp.task("less",
    function () {
        return gulp.src(paths.webroot + 'less/*.less')
            .pipe(less({
                paths: [path.join(__dirname, 'less', 'includes')]
            }))
            .pipe(gulp.dest(paths.webroot + 'css'));
    });
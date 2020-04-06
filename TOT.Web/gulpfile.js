"use strict";
var gulp = require("gulp"),
    path = require("path"),
    less = require("gulp-less"),
    gulp_cahced = require("gulp-cached");

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

gulp.task("material-design-icons",
    function () {
        return gulp.src("node_modules/material-design-icons/**")
            .pipe(gulp_cahced("node_modules/material-design-icons/**"))
            .pipe(gulp.dest(paths.webroot + "material-design-icons"));
    });
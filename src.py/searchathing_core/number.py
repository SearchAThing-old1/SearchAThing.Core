"""
* SearchAThing.Core, Copyright(C) 2015-2017 Lorenzo Delana, License under MIT
*
* The MIT License(MIT)
* Copyright(c) 2015-2017 Lorenzo Delana, https://searchathing.com
*
* Permission is hereby granted, free of charge, to any person obtaining a
* copy of this software and associated documentation files (the "Software"),
* to deal in the Software without restriction, including without limitation
* the rights to use, copy, modify, merge, publish, distribute, sublicense,
* and/or sell copies of the Software, and to permit persons to whom the
* Software is furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
* FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
* DEALINGS IN THE SOFTWARE.

"""

import sys
import numpy


def normalized_length_tolerance() -> float:
    """ tolerance to be used when comparing normalized vectors """
    return 1e-4


def equals_auto_tol(x: float, y: float, precision: float = 1e-6) -> bool:
    """ Returns true if two numbers are equals using a default tolerance of 1e-6 about the smaller one. """
    return abs(x - y) < min(x, y) * precision;


def mround(value: float, multiple: float) -> float:
    """ Round the given value using the multiple basis """
    if abs(multiple) < sys.float_info.min:
        return value

    p = round(value / multiple, 0)

    return p * multiple


def equals_tol(tol: float, x: float, y: float) -> bool:
    return abs(x - y) <= tol


def great_than_tol(tol: float, x: float, y: float) -> bool:
    return x > y and not equals_tol(tol, x, y)


def great_than_or_equals_tol(tol: float, x: float, y: float) -> bool:
    return x > y or equals_tol(tol, x, y)


def less_than_tol(tol: float, x: float, y: float) -> bool:
    return x < y and not equals_tol(tol, x, y)


def less_than_or_equals_tol(tol: float, x: float, y: float) -> bool:
    return x < y or equals_tol(tol, x, y)


def to_deg(angle_rad: float) -> float:
    """ convert given angle(rad) to degree """
    return angle_rad / numpy.pi * 180


def to_rad(angle_grad: float) -> float:
    """ convert given angle(grad) to radians """
    return angle_grad / 180 * numpy.pi

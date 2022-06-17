# by Falk Meyer, 2021-03-21

# imports
# =======================
# system
import unittest
# third-party
import geopandas
# local
from analysisUtils.SpatialAnalysisCase import SpatialAnalysisCase

# logic
# ======================

class SpatialAnalysisCaseTests(unittest.TestCase):
    """
    Class for SpatialAnalysisCase unit tests
    """

    def test_construction(self):
        # input
        subjects = geopandas.GeoDataFrame()
        subjectsIdField = 'id'
        origin = geopandas.GeoDataFrame()

        # logic
        case = SpatialAnalysisCase(subjects, subjectsIdField, origin)

        # tests
        self.assertEqual(type(case.subjects), geopandas.GeoDataFrame)
        self.assertEqual(case.subjectsIdField, subjectsIdField)
        self.assertEqual(type(case.origin), geopandas.GeoDataFrame)

    def test_construction_wrong_input(self):
        # input
        subjects = {} # should be GeoDataFrame
        subjectsIdField = 'id'
        origin = geopandas.GeoDataFrame()

        # tests
        with self.assertRaises(AssertionError):
             SpatialAnalysisCase(subjects, subjectsIdField, origin)


if __name__ == '__main__':
    unittest.main()

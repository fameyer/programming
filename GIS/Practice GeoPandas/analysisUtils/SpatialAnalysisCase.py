# by Falk Meyer, 2021-03-20

# imports
# =======================
# third-party
import geopandas
# local
from .AnalysisCase import AnalysisCase

# logic
# ======================

class SpatialAnalysisCase(AnalysisCase):
    """
    Class for a spatial buffer analysis
    """

    def __init__(self, subjects, subjectsIdField, origin):
        """
        Set objects to be analyzed
        :param subjects: The subjects to be analyzed (GeoDataFrame)
        :param subjectsIdField: The unique id field of the subjects
        :param origin: The geometric origin of the spatial analysis (GeoDataFrame)
        """
        super().__init__()

        # assertions
        assert isinstance(subjects, geopandas.GeoDataFrame)
        assert isinstance(origin, geopandas.GeoDataFrame)

        self.subjects = subjects
        self.subjectsIdField = subjectsIdField
        self.origin = origin

    def analyze(self, inputBuffer, capStyle):
        """
        Start an spatial analysis, creating buffer for the origin geometry and determining the subjects,
        which intersect with the buffered geometry
        :param inputBuffer: The requested buffer in meter
        :param capStyle: The buffer style (refer to https://geopandas.org/docs/reference/api/geopandas.GeoSeries.buffer.html#geopandas.GeoSeries.buffer)
        :return:
        """
        # create buffered geoseries
        bufferedOriginFrame = geopandas.GeoDataFrame({'geometry': self.origin['geometry'].buffer(inputBuffer, cap_style=capStyle)})

        # Get buildings affected positively by tunnel
        intersectedFrame = geopandas.overlay(bufferedOriginFrame, self.subjects, how='intersection')

        # get ids of intersected features
        affectedFeaturesIds = intersectedFrame[self.subjectsIdField]

        # return complete subjects by id
        return self.subjects[self.subjects[self.subjectsIdField].isin(affectedFeaturesIds)]
